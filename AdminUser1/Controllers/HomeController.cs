using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdminUser1.Models;

namespace AdminUser1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        [HttpGet]
        public ActionResult UM()
        {
            SimpleUserModel m = new SimpleUserModel { Id = Guid.NewGuid().ToString(), Email = "Test", Counter = 1 };
            return View(m);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult UM([Bind(include:"Email,Counter")] SimpleUserModel m)
        {
            m.Counter++;
            return PartialView("UMPartial", m);
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

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
