using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsNews.Data;
using SportsNews.Data.Models;
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
        private readonly ApplicationDbContext applicationDbContext;
        private readonly InfoArchitectureUnitOfWork infoArchitectureUnitOfWork;
        private readonly IEnumerable<AdminMenu> menuItems;

        public AdminController(UserPhotoUnitOfWork userPhotoUnitOfWork, ApplicationDbContext applicationDbContext, InfoArchitectureUnitOfWork infoArchitectureUnitOfWork)
        {
            this.applicationDbContext = applicationDbContext;
            this.userPhotoUnitOfWork = userPhotoUnitOfWork;
            this.infoArchitectureUnitOfWork = infoArchitectureUnitOfWork;
            menuItems = this.applicationDbContext.AdminMenuItems.ToList();
        }

        public IActionResult Index()
        {
            var model = new LayoutViewModel("Administration zone", true,
                this.userPhotoUnitOfWork.UserPhotos.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                Menu = this.menuItems
            };
            return View(model);
        }

        //Information architecture
        [HttpGet]
        public IActionResult InfoArchitecture()
        {
            var modelInfo = new InfoArchitectureViewModel
            {
                Categories = infoArchitectureUnitOfWork.Categories.GetItems(),
                SelectedCategoryId = 0,
                SubCategories =  null, //infoArchitectureUnitOfWork.SubCategories.GetItems(),
                SelectedSubCategoryId = 0,
                Teams = null //infoArchitectureUnitOfWork.Teams.GetItems()
            };
            var model = new LayoutViewModel<InfoArchitectureViewModel>(modelInfo, "Information architecture", true,
                this.userPhotoUnitOfWork.UserPhotos.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>()) 
            { 
                Menu = this.menuItems 
            };
            return View(model);
        }

        [HttpGet]
        [Route("/Admin/InfoArchitecture/{id}")]
        public IActionResult InfoArchitecture(int id, int subId)
        {
            var modelInfo = new InfoArchitectureViewModel
            {
                Categories = infoArchitectureUnitOfWork.Categories.GetItems(),
                SelectedCategoryId = id,
                SubCategories = infoArchitectureUnitOfWork.SubCategories.GetItemsByCategoryId(id),
                SelectedSubCategoryId = subId,
                Teams = infoArchitectureUnitOfWork.Teams.GetItemsBySubCategoryId(subId),
                Name = subId==0 ? infoArchitectureUnitOfWork.Categories.GetItemByID(id).Name : infoArchitectureUnitOfWork.SubCategories.GetItemByID(subId).Name
            };
            var model = new LayoutViewModel<InfoArchitectureViewModel>(modelInfo, "Information architecture", true,
                this.userPhotoUnitOfWork.UserPhotos.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                Menu = this.menuItems
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddCategory(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.PageModel.Name))
            {
                this.infoArchitectureUnitOfWork.Categories.InsertItem(new Category()
                {
                    Name = model.PageModel.Name,
                    IsVisible = true,
                    IsStatic = false
                });
                this.infoArchitectureUnitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin");
        }

        [HttpPost]
        public IActionResult AddSubCategory(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.PageModel.Name))
            {
                this.infoArchitectureUnitOfWork.SubCategories.InsertItem(new SubCategory() 
                { 
                    CategoryId = model.PageModel.SelectedCategoryId,
                    Name = model.PageModel.Name,
                    IsVisible = true
                });
                this.infoArchitectureUnitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new { id = model.PageModel.SelectedCategoryId });
        }

        [HttpPost]
        public IActionResult AddTeam(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.PageModel.Name))
            {
                this.infoArchitectureUnitOfWork.Teams.InsertItem(new Team()
                {
                    SubCategoryId = model.PageModel.SelectedSubCategoryId,
                    Name = model.PageModel.Name,
                    IsVisible = true,
                });
                this.infoArchitectureUnitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new { id = model.PageModel.SelectedCategoryId, subId = model.PageModel.SelectedSubCategoryId });
        }

        public IActionResult EditCategory(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.PageModel.Name))
            {
                this.infoArchitectureUnitOfWork.Categories.UpdateItem(new Category()
                {
                    Id = model.PageModel.SelectedCategoryId,
                    Name = model.PageModel.Name,
                    IsVisible = true
                });
                this.infoArchitectureUnitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new { id = model.PageModel.SelectedCategoryId });
        }
    }
}
