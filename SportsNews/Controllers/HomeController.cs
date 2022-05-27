﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new LayoutViewModel("Home"));
        }

        public IActionResult Privacy()
        {
            return View(new LayoutViewModel("Privacy"));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new LayoutViewModel<ErrorViewModel>(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }, 
                "Error"));
        }
    }
}
