using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsNews.Data;
using SportsNews.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext applicationDbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            this.applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View(new LayoutViewModel("Home", false, UserInfoHelper.GetUserImage(applicationDbContext, User)));
        }

        public IActionResult Privacy()
        {
            return View(new LayoutViewModel("Privacy", false, UserInfoHelper.GetUserImage(applicationDbContext, User)));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new LayoutViewModel<ErrorViewModel>(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier },
                "Error", false, UserInfoHelper.GetUserImage(applicationDbContext, User)));
        }
    }
}
