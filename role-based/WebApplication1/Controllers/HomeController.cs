using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[AllowAnonymous]

        [Authorize(Roles = "dummy_2")] // access
        //[Authorize(Roles = "dummy_3")] // no access

        // OR: role dummy_2 OR dummy_3 needed
        //[Authorize(Roles = "dummy_2,dummy_3")] // OR

        // AND: if on separate line: role dummy_2 AND dummy_3 needed
        //[Authorize(Roles = "dummy_2")]
        //[Authorize(Roles = "dummy_3")]

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}