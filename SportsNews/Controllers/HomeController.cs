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
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View(new LayoutViewModel("Home", false, 
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>()));
        }

        public IActionResult Privacy()
        {
            return View(new LayoutViewModel("Privacy", false, 
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new LayoutViewModel<ErrorViewModel>(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier },
                "Error", false, 
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>()));
        }
    }
}
