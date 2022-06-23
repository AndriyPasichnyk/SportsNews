using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SportsNews.Data;
using SportsNews.Data.Models;
using SportsNews.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IOptions<RequestLocalizationOptions> locOptions;
        private readonly IEnumerable<AdminMenu> menuItems;
        private readonly IEnumerable<Language> languages;

        public AdminController(ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, IOptions<RequestLocalizationOptions> locOptions)
        {
            this.applicationDbContext = applicationDbContext;
            this.unitOfWork = unitOfWork;
            this.locOptions = locOptions;
            menuItems = this.unitOfWork.AdminMenu.GetItems().ToList();
            languages = this.unitOfWork.Languages.GetItems().ToList();
        }

        public IActionResult Index()
        {
            var model = new LayoutViewModel("Administration zone", true,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                Menu = this.menuItems,
                Languages = this.languages
            };
            return View(model);
        }

        #region Information architecture
        [HttpGet]
        public IActionResult InfoArchitecture()
        {
            var modelInfo = new InfoArchitectureViewModel
            {
                Categories = unitOfWork.Categories.GetItems(),
                SubCategories = null,
                Teams = null,
                SelectedCategory = new AdminMenuItemViewModel { Id = 0 },
                SelectedSubCategory = new AdminMenuItemViewModel { Id = 0 },
                SelectedTeam = new AdminMenuItemViewModel { Id = 0 }
            };
            var model = new LayoutViewModel<InfoArchitectureViewModel>(modelInfo, "Information architecture", true,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                Menu = this.menuItems,
                Languages = this.languages
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
                SelectedCategory = new AdminMenuItemViewModel
                {
                    Id = id,
                    Name = id != 0 ? unitOfWork.Categories.GetItemByID(id).Name : string.Empty
                },
                SelectedSubCategory = new AdminMenuItemViewModel
                {
                    Id = subId,
                    Name = subId != 0 ? unitOfWork.SubCategories.GetItemByID(subId).Name : string.Empty
                },
                SelectedTeam = new AdminMenuItemViewModel
                {
                    Id = tId,
                    Name = tId != 0 ? unitOfWork.Teams.GetItemByID(tId).Name : string.Empty
                }
            };
            var model = new LayoutViewModel<InfoArchitectureViewModel>(modelInfo, "Information architecture", true,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                Menu = this.menuItems,
                Languages = this.languages
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
                var item = this.unitOfWork.Categories.GetItemByID(model.PageModel.SelectedCategory.Id);
                item.Name = model.PageModel.SelectedCategory.Name;
                this.unitOfWork.Categories.UpdateItem(item);
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
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.PageModel.SelectedSubCategory.Name))
            {
                var item = this.unitOfWork.SubCategories.GetItemByID(model.PageModel.SelectedSubCategory.Id);
                item.Name = model.PageModel.SelectedSubCategory.Name;
                this.unitOfWork.SubCategories.UpdateItem(item);
                this.unitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new
            {
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
                var item = this.unitOfWork.Teams.GetItemByID(model.PageModel.SelectedTeam.Id);
                item.Name = model.PageModel.SelectedTeam.Name;
                this.unitOfWork.Teams.UpdateItem(item);
                this.unitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new
            {
                id = model.PageModel.SelectedCategory.Id,
                subId = model.PageModel.SelectedSubCategory.Id,
                tId = model.PageModel.SelectedTeam.Id
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

        [HttpPost]
        public IActionResult HideOrUnhideCategory(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var item = this.unitOfWork.Categories.GetItemByID(model.PageModel.SelectedCategory.Id);
            if (item != null)
            {
                item.IsVisible = !item.IsVisible;
                this.unitOfWork.Categories.UpdateItem(item);
                this.unitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new
            {
                id = model.PageModel.SelectedCategory.Id
            });
        }

        [HttpPost]
        public IActionResult HideOrUnhideSubCategory(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var item = this.unitOfWork.SubCategories.GetItemByID(model.PageModel.SelectedSubCategory.Id);
            if (item != null)
            {
                item.IsVisible = !item.IsVisible;
                this.unitOfWork.SubCategories.UpdateItem(item);
                this.unitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new
            {
                id = model.PageModel.SelectedCategory.Id,
                subId = model.PageModel.SelectedSubCategory.Id
            });
        }

        [HttpPost]
        public IActionResult HideOrUnhideTeam(LayoutViewModel<InfoArchitectureViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var item = this.unitOfWork.Teams.GetItemByID(model.PageModel.SelectedTeam.Id);
            if (item != null)
            {
                item.IsVisible = !item.IsVisible;
                this.unitOfWork.Teams.UpdateItem(item);
                this.unitOfWork.Save();
            }

            return RedirectToAction("InfoArchitecture", "Admin", new
            {
                id = model.PageModel.SelectedCategory.Id,
                subId = model.PageModel.SelectedSubCategory.Id,
                tId = model.PageModel.SelectedTeam.Id
            });
        }
        #endregion

        #region Teams
        [HttpGet]
        public IActionResult Teams()
        {
            var modelTeams = new TeamsViewModel
            {
                Categories = unitOfWork.Categories.GetItems(),
                SubCategories = unitOfWork.SubCategories.GetItems(),
                Teams = unitOfWork.Teams.GetItems(),
            };
            var model = new LayoutViewModel<TeamsViewModel>(modelTeams, "Teams", true,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                Menu = this.menuItems,
                Languages = this.languages
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Teams(LayoutViewModel<TeamsViewModel> model)
        {
            var modelTeams = new TeamsViewModel
            {
                Categories = model.PageModel.SelectedCategory.Id == 0 ? unitOfWork.Categories.GetItems() : unitOfWork.Categories.GetItemsByID(model.PageModel.SelectedCategory.Id),
                SubCategories = model.PageModel.SelectedCategory.Id == 0 ? unitOfWork.SubCategories.GetItems() : unitOfWork.SubCategories.GetItemsByCategoryId(model.PageModel.SelectedCategory.Id),
                Teams = model.PageModel.SelectedSubCategory.Id == 0 ? unitOfWork.Teams.GetItems() : unitOfWork.Teams.GetItemsBySubCategoryId(model.PageModel.SelectedSubCategory.Id),
                SelectedCategory = model.PageModel.SelectedCategory,
                SelectedSubCategory = model.PageModel.SelectedSubCategory,
                SelectedTeam = model.PageModel.SelectedTeam
            };
            var modelNew = new LayoutViewModel<TeamsViewModel>(modelTeams, "Teams", true,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                Menu = this.menuItems,
                Languages = this.languages
            };
            return View(modelNew);
        }
        #endregion

        #region Languages
        [HttpGet]
        public IActionResult Languages()
        {
            return RedirectToAction("Languages", "Admin", new { id = 0 });
        }

        [HttpGet]
        [Route("/Admin/Languages/{id}")]
        public IActionResult Languages(int id)
        {
            var lModel = id == 0 ? new Language() : unitOfWork.Languages.GetItemByID(id);
            var model = new LayoutViewModel<Language>(lModel, "Languages", true,
                this.unitOfWork.UsersPhoto.GetUserPhotoByUserName(User.Identity.Name)?.ProfilePicture ?? Array.Empty<byte>())
            {
                Menu = this.menuItems,
                Languages = this.languages
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult HideOrUnhideLanguage(LayoutViewModel<Language> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var item = this.unitOfWork.Languages.GetItemByID(model.PageModel.Id);
            if (item != null)
            {
                item.IsEnabled = !item.IsEnabled;
                this.unitOfWork.Languages.UpdateItem(item);
                this.unitOfWork.Save();
            }

            return RedirectToAction("Languages", "Admin", new { id = model.PageModel.Id });
        }

        [HttpPost]
        public IActionResult DeleteLanguage(LayoutViewModel<Language> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var lang = this.unitOfWork.Languages.GetItemByID(model.PageModel.Id);
            if (lang != null)
            {
                this.unitOfWork.Languages.DeleteItem(model.PageModel.Id);
                this.unitOfWork.Save();

                var culture = new CultureInfo(lang.Abbreviation);
                if (this.locOptions.Value.SupportedCultures.Contains(culture))
                {
                    var list = this.locOptions.Value.SupportedCultures.ToList();
                    list.Remove(culture);
                    this.locOptions.Value.SupportedCultures = list;
                    this.locOptions.Value.SupportedUICultures = list;
                }
            }

            return RedirectToAction("Languages", "Admin", new { id = 0 });
        }

        [HttpPost]
        public IActionResult AddLanguage(LayoutViewModel<Language> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.PageModel.Name) && !string.IsNullOrEmpty(model.PageModel.Abbreviation))
            {
                this.unitOfWork.Languages.InsertItem(new Language()
                {
                    Name = model.PageModel.Name,
                    Abbreviation = model.PageModel.Abbreviation,
                    IsEnabled = true
                });
                this.unitOfWork.Save();

                var culture = new CultureInfo(model.PageModel.Abbreviation);
                if (!this.locOptions.Value.SupportedCultures.Contains(culture))
                {
                    var list = this.locOptions.Value.SupportedCultures.ToList();
                    list.Add(culture);
                    this.locOptions.Value.SupportedCultures = list;
                    this.locOptions.Value.SupportedUICultures = list;
                }
            }

            return RedirectToAction("Languages", "Admin", new { id = 0 });
        }
        #endregion


    }
}
