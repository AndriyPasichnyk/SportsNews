using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SportsNews.Data;
using SportsNews.Data.Models;
using SportsNews.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Controllers
{
    [Authorize(Policy = Policies.All)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IStringLocalizer<HomeController> stringLocalizer;
        private readonly IEnumerable<Language> languages;
        private readonly IEnumerable<Category> menuItems;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IStringLocalizer<HomeController> stringLocalizer)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.stringLocalizer = stringLocalizer;
            this.languages = this.unitOfWork.Languages.GetItems().ToList();
            this.menuItems = this.unitOfWork.Categories.GetItems().ToList();
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = new LayoutViewModel(this.stringLocalizer["Home Page"], false,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                UserMenu = this.menuItems,
                Languages = this.languages
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            var model = new LayoutViewModel(this.stringLocalizer["Privacy Policy"], false,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                UserMenu = this.menuItems,
                Languages = this.languages 
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var model = new LayoutViewModel<ErrorViewModel>(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier },
                stringLocalizer["Error"], 
                false,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                UserMenu = this.menuItems,
                Languages = this.unitOfWork.Languages.GetItems()
            };
            return View(model);
        }


        private LayoutViewModel GetComposedUserModel(TeamsViewModel model)
        {
            return new LayoutViewModel<TeamsViewModel>(model, "Teams", true,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                UserMenu = this.menuItems,
                Languages = this.languages
            };
        }
    }
}
