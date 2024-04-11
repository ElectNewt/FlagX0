using FlagX0.Web.Business.Mappers;
using FlagX0.Web.Business.UserInfo;
using FlagX0.Web.Data;
using FlagX0.Web.Data.Entities;
using FlagX0.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagX0.Web.Business.UseCases.Flags
{
    public class GetPaginatedFlagsUseCase(ApplicationDbContext applicationDbContext)
    {
        public async Task<Result<Pagination<FlagDto>>> Execute(string? search, int page, int size)
        => await ValidatePage(page)
            .Fallback(_ =>
            {
                page = 1;
                return Result.Unit;
            })
            .Bind(_ => ValidatePageSize(size)
                .Fallback(_ =>
                {
                    size = 5;
                    return Result.Unit;
                })
            ).Async()
            .Bind(x => GetFromDb(search, page, size))
                .Map(x => x.ToDto())
                .Combine(x => TotalElements(search))
            .Map(x => new Pagination<FlagDto>(x.Item1, x.Item2, size, page, search));

        private Result<Unit> ValidatePage(int page)
        {
            if (page < 1)
                return Result.Failure("page not supported");

            return Result.Unit;
        }

        private Result<Unit> ValidatePageSize(int pageSize)
        {
            int[] allowedValues = [5, 10, 15];
            if (!allowedValues.Contains(pageSize))
            {
                return Result.Failure("page size not supported");
            }
            return Result.Unit;
        }

        private async Task<Result<List<FlagEntity>>> GetFromDb(string? search, int page, int size)
        {
            IQueryable<FlagEntity> query = applicationDbContext.Flags
                .Skip(size * (page - 1))
                .Take(size);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a => a.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase));
            }

            return await query.ToListAsync();
        }

        private async Task<Result<int>> TotalElements(string? search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                return await applicationDbContext.Flags
                    .Where(a => a.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase))
                    .CountAsync();
            }
            return await applicationDbContext.Flags.CountAsync();
        }
    }
}
