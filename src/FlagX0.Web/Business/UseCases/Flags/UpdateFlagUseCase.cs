using FlagX0.Web.Business.UserInfo;
using FlagX0.Web.Data.Entities;
using FlagX0.Web.Data;
using FlagX0.Web.DTOs;
using ROP;
using Microsoft.EntityFrameworkCore;
using FlagX0.Web.Business.Mappers;

namespace FlagX0.Web.Business.UseCases.Flags
{
    public class UpdateFlagUseCase(ApplicationDbContext applicationDbContext, IFlagUserDetails userDetails)
    {
        public async Task<Result<FlagDto>> Execute(FlagDto flagDto)
        => await VerifyIstheOnlyOneWithThatName(flagDto)
                .Bind(x => GetFromDb(x.Id))
                .Bind(x => Update(x, flagDto))
                .Map(x => x.ToDto());

        private async Task<Result<FlagDto>> VerifyIstheOnlyOneWithThatName(FlagDto dto)
        {
            bool alreadyExist = await applicationDbContext.Flags
                .AnyAsync(a => a.UserId == userDetails.UserId
                && a.Name.Equals(dto.Name, StringComparison.CurrentCultureIgnoreCase)
                && a.Id != dto.Id);

            if (alreadyExist)
            {
                return Result.Failure<FlagDto>("Flag with the same name already exist");
            };

            return dto;
        }
		

	private async Task<Result<FlagEntity>> GetFromDb(int id)
        => await applicationDbContext.Flags
                .Where(a => a.UserId == userDetails.UserId
                        && a.Id == id)
                .SingleAsync();

        private async Task<Result<FlagEntity>> Update(FlagEntity entity, FlagDto flagDto)
        {
            entity.Value = flagDto.IsEnabled;
            entity.Name = flagDto.Name;
            await applicationDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
