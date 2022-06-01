using Microsoft.AspNetCore.Mvc;
using SportsNews.Data;
using SportsNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Controllers
{
    public class AdminController : Controller
    {
        public ApplicationDbContext applicationDbContext { get; set; }

        public AdminController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View(new LayoutViewModel("Administration zone", true, UserInfoHelper.GetUserImage(applicationDbContext, User)));
        }
    }
}
