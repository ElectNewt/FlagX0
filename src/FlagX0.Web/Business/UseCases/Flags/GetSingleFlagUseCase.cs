using FlagX0.Web.Business.UserInfo;
using FlagX0.Web.Data.Entities;
using FlagX0.Web.Data;
using FlagX0.Web.DTOs;
using ROP;
using Microsoft.EntityFrameworkCore;
using FlagX0.Web.Business.Mappers;

namespace FlagX0.Web.Business.UseCases.Flags
{
	public class GetSingleFlagUseCase(ApplicationDbContext applicationDbContext)
	{
		public async Task<Result<FlagDto>> Execute(string flagName)
		=> await GetFromDb(flagName)
				 .Bind(flag => flag ?? Result.NotFound<FlagEntity>("FlagDoes not exist"))
				.Map(x => x.ToDto());

		private async Task<Result<FlagEntity?>> GetFromDb(string flagname)
		=> await applicationDbContext.Flags
				.Where(a => a.Name.Equals(flagname, StringComparison.InvariantCultureIgnoreCase))
				.AsNoTracking()
				.FirstOrDefaultAsync();
	}
}
