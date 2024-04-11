using FlagX0.Web.Business.UseCases;
using FlagX0.Web.DTOs;
using FlagX0.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;

namespace FlagX0.Web.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true)]
	[Authorize]
	[Route("[controller]")]
	public class FlagsController(FlagsUseCases flags) : Controller
	{
		[HttpGet("create")]
		public IActionResult Create()
		{
			return View(new FlagViewModel());
		}

		[HttpPost("create")]
		public async Task<IActionResult> AddFlagToDatabase(FlagViewModel request)
		{

			Result<bool> isCreated = await flags.Add.Execute(request.Name, request.IsEnabled);

			if (isCreated.Success)
			{
				return RedirectToAction("Index");
			}

			return View("Create", new FlagViewModel()
			{
				Error = isCreated.Errors.First().Message,
				IsEnabled = request.IsEnabled,
				Name = request.Name
			});
		}

		[HttpGet("{flagName}")]
		public async Task<IActionResult> GetSingle(string flagName, string? message)
		{
			var singleFlag = await flags.Get.Execute(flagName);

			return View("SingleFlag", new SingleFlagViewModel()
			{
				Flag = singleFlag.Value,
				Message = message
			});
		}

		[HttpPost("{flagName}")]
		public async Task<IActionResult> Update(FlagDto flag)
		{
			var updatedFlag = await flags.Update.Execute(flag);

			if (updatedFlag.Success)
			{
				return RedirectToAction("GetSingle", new { flagname = flag.Name, message = "Updated correctly" });
			}

			return View("SingleFlag", new SingleFlagViewModel()
			{
				Flag = flag,
				Message = updatedFlag.Errors.First().Message
			});
		}

		[HttpGet("delete/{flagName}")]
		public async Task<IActionResult> Delete(string flagName)
		{
			var isDeleted = await flags.Delete.Execute(flagName);

			if (isDeleted.Success)
			{
				return RedirectToAction("");
			}

			return RedirectToAction("GetSingle", new
			{
				flagname = flagName,
				message = "Updated correctly"
			});
		}


		[HttpGet("")]
		[HttpGet("{page:int}")]
		public async Task<IActionResult> Index(string? search, int page = 1, int size = 5)
		{
			var listFlags = (await flags.GetPaginated
					.Execute(search, page, size)).Throw();

			return View(new FlagIndexViewModel()
			{
				Pagination = listFlags
			});
		}
	}
}
