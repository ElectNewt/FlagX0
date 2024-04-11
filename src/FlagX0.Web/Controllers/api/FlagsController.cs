using FlagX0.Web.Business.UseCases.Flags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;
using ROP.APIExtensions;

namespace FlagX0.Web.Controllers.api
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class FlagsController(GetSingleFlagUseCase getSingleFlag) : ControllerBase
	{
		[ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status404NotFound)]
		[HttpGet("{flagname}")]
		public async Task<IActionResult> GetSingleFlag(string flagname)
		=> await getSingleFlag
			.Execute(flagname)
			.Map(a => a.IsEnabled)
			.ToActionResult();
	}
}
