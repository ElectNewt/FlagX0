using FlagX0.Web.Business.UseCases.Flags;

namespace FlagX0.Web.Business.UseCases
{
	public record class FlagsUseCases(AddFlagUseCase Add, GetPaginatedFlagsUseCase GetPaginated,
	GetSingleFlagUseCase Get, UpdateFlagUseCase Update, DeleteFlagUseCase Delete)
	{
	}
}
