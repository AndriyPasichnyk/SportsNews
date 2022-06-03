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
        private readonly UserPhotoUnitOfWork userPhotoUnitOfWork;
        public AdminController(UserPhotoUnitOfWork userPhotoUnitOfWork)
        {
            this.userPhotoUnitOfWork = userPhotoUnitOfWork;
        }

        public IActionResult Index()
        {
            return View(new LayoutViewModel("Administration zone", true, this.userPhotoUnitOfWork.UserPhotos.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture));
        }
    }
}
