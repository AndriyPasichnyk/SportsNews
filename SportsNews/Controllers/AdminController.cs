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
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEnumerable<AdminMenu> menuItems;

        public AdminController(ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork)
        {
            this.applicationDbContext = applicationDbContext;
            this.unitOfWork = unitOfWork;
            menuItems = this.unitOfWork.AdminMenu.GetItems().ToList();
        }

        public IActionResult Index()
        {
            var model = new LayoutViewModel("Administration zone", true,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
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
                Categories = unitOfWork.Categories.GetItems(),
                SubCategories =  null, 
                Teams = null, 
                SelectedCategory = new AdminMenuItemViewModel { Id = 0 },
                SelectedSubCategory = new AdminMenuItemViewModel { Id = 0 },
                SelectedTeam = new AdminMenuItemViewModel { Id = 0 }
            };
            var model = new LayoutViewModel<InfoArchitectureViewModel>(modelInfo, "Information architecture", true,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>()) 
            { 
                Menu = this.menuItems 
            };
            return View(model);
        }

        [HttpGet]
        [Route("/Admin/InfoArchitecture/{id}")]
        public IActionResult InfoArchitecture(int id, int subId, int tId)
        {
            var modelInfo = new InfoArchitectureViewModel
            {
                Categories = unitOfWork.Categories.GetItems(),
                SubCategories = unitOfWork.SubCategories.GetItemsByCategoryId(id),
                Teams = unitOfWork.Teams.GetItemsBySubCategoryId(subId),
                SelectedCategory = new AdminMenuItemViewModel { Id = id, Name = id != 0 ? unitOfWork.Categories.GetItemByID(id).Name : string.Empty },
                SelectedSubCategory = new AdminMenuItemViewModel { Id = subId, Name = subId!=0 ? unitOfWork.SubCategories.GetItemByID(subId).Name : string.Empty },
                SelectedTeam = new AdminMenuItemViewModel { Id = tId, Name = tId!=0 ? unitOfWork.Teams.GetItemByID(tId).Name : string.Empty }
            };
            var model = new LayoutViewModel<InfoArchitectureViewModel>(modelInfo, "Information architecture", true,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
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

            if (!string.IsNullOrEmpty(model.PageModel.SelectedCategory.NewName))
            {
                this.unitOfWork.Categories.InsertItem(new Category()
                {
                    Name = model.PageModel.SelectedCategory.NewName,
                    IsVisible = true,
                    IsStatic = false
                });
                this.unitOfWork.Save();
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

            if (!string.IsNullOrEmpty(model.PageModel.SelectedSubCategory.NewName))
            {
                this.unitOfWork.SubCategories.InsertItem(new SubCategory() 
                { 
                    CategoryId = model.PageModel.SelectedCategory.Id,
                    Name = model.PageModel.SelectedSubCategory.NewName,
                    IsVisible = true
                });
                this.unitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new 
            { 
                id = model.PageModel.SelectedCategory.Id 
            });
        }

        [HttpPost]
        public IActionResult AddTeam(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.PageModel.SelectedTeam.NewName))
            {
                this.unitOfWork.Teams.InsertItem(new Team()
                {
                    SubCategoryId = model.PageModel.SelectedSubCategory.Id,
                    Name = model.PageModel.SelectedTeam.NewName,
                    IsVisible = true,
                });
                this.unitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new
            {
                id = model.PageModel.SelectedCategory.Id, 
                subId = model.PageModel.SelectedSubCategory.Id
            });
        }

        [HttpPost]
        public IActionResult EditCategory(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.PageModel.SelectedCategory.Name))
            {
                this.unitOfWork.Categories.UpdateItem(new Category()
                {
                    Id = model.PageModel.SelectedCategory.Id,
                    Name = model.PageModel.SelectedCategory.Name,
                    IsVisible = true
                });
                this.unitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new
            {
                id = model.PageModel.SelectedCategory.Id 
            });
        }

        [HttpPost]
        public IActionResult EditSubCategory(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            { 
                return View(model); }

            if(!string.IsNullOrEmpty(model.PageModel.SelectedSubCategory.Name))
            {
                this.unitOfWork.SubCategories.UpdateItem(new SubCategory()
                {
                    Id = model.PageModel.SelectedSubCategory.Id,
                    Name = model.PageModel.SelectedSubCategory.Name,
                    IsVisible = true,
                    CategoryId = model.PageModel.SelectedCategory.Id
                });
                this.unitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new { 
                id = model.PageModel.SelectedCategory.Id, 
                subId = model.PageModel.SelectedSubCategory.Id 
            });
        }

        [HttpPost]
        public IActionResult EditTeam(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.PageModel.SelectedTeam.Name))
            {
                this.unitOfWork.Teams.UpdateItem(new Team()
                {
                    Id = model.PageModel.SelectedTeam.Id,
                    Name = model.PageModel.SelectedTeam.Name,
                    IsVisible = true,
                    SubCategoryId = model.PageModel.SelectedSubCategory.Id
                });
                this.unitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new {
                id = model.PageModel.SelectedCategory.Id, 
                subId = model.PageModel.SelectedSubCategory.Id,
                tId =model.PageModel.SelectedTeam.Id
            });
        }

        [HttpPost]
        public IActionResult DeleteCategory(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.unitOfWork.Categories.DeleteItem(model.PageModel.SelectedCategory.Id);
            this.unitOfWork.Save();

            return RedirectToAction("InfoArchitecture", "Admin");
        }

        [HttpPost]
        public IActionResult DeleteSubCategory(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.unitOfWork.SubCategories.DeleteItem(model.PageModel.SelectedSubCategory.Id);
            this.unitOfWork.Save();

            return RedirectToAction("InfoArchitecture", "Admin", new 
            { 
                id = model.PageModel.SelectedCategory.Id 
            });
        }

        [HttpPost]
        public IActionResult DeleteTeam(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.unitOfWork.Teams.DeleteItem(model.PageModel.SelectedTeam.Id);
            this.unitOfWork.Save();

            return RedirectToAction("InfoArchitecture", "Admin", new 
            { 
                id = model.PageModel.SelectedCategory.Id, 
                subId = model.PageModel.SelectedSubCategory.Id 
            });
        }
    }
}
