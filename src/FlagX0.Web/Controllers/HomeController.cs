using FlagX0.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlagX0.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
		public IActionResult Index()
        {
            return View(new IndexViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Example(string text)
        {
            return View("index", new IndexViewModel()) ;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
