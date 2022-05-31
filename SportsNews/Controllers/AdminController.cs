using Microsoft.AspNetCore.Mvc;
using SportsNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View(new LayoutViewModel("Administration zone", true));
        }
    }
}
