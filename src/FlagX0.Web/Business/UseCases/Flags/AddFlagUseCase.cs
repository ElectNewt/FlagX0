using FlagX0.Web.Business.UserInfo;
using FlagX0.Web.Data;
using FlagX0.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagX0.Web.Business.UseCases.Flags
{
    public class AddFlagUseCase(ApplicationDbContext applicationDbContext, IFlagUserDetails flagUserDetails)
    {
        public async Task<Result<bool>> Execute(string flagName, bool isActive)
        => await ValidateFlag(flagName)
                .Bind(x => ADdFlagToDatabase(x, isActive));

        private async Task<Result<string>> ValidateFlag(string flagName)
        {
            bool flagExist = await applicationDbContext.Flags
                .Where(a => a.Name.Equals(flagName, StringComparison.InvariantCultureIgnoreCase))
                .AnyAsync();

            if (flagExist)
            {
                return Result.Failure<string>("Flag Name Already Exist");
            }
            return flagName;
        }

        private async Task<Result<bool>> ADdFlagToDatabase(string flagName, bool isActive)
        {
            FlagEntity entity = new()
            {
                Name = flagName,
                UserId = flagUserDetails.UserId,
                Value = isActive
            };

            _ = await applicationDbContext.Flags.AddAsync(entity);
            await applicationDbContext.SaveChangesAsync();

            return true;
        }
    }
}
