using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using SportsNews.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Models
{
    public class InfoArchitectureBinder : IModelBinder
    {
        private readonly IUnitOfWork unitOfWork;

        public InfoArchitectureBinder(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue("PageModel.SelectedTeam.NewName");
            var teamName = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(teamName))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Team name should not be empty!");
            }

            valueProviderResult = bindingContext.ValueProvider.GetValue("PageModel.SelectedSubCategory.Id");
            var value = valueProviderResult.FirstValue;
            if (!int.TryParse(value, out var subId))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "SubCategory Id must be an integer.");
            }
            else if (this.unitOfWork.SubCategories.GetItemByID(subId) == null)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "SubCategory Id missed in database.");
            }

            valueProviderResult = bindingContext.ValueProvider.GetValue("PageModel.SelectedCategory.Id");
            value = valueProviderResult.FirstValue;
            int.TryParse(value, out var catId);

            bindingContext.Result = ModelBindingResult.Success(
                new LayoutViewModel<InfoArchitectureViewModel>(
                    new InfoArchitectureViewModel()
                    {
                        Categories = unitOfWork.Categories.GetItems(),
                        SubCategories = unitOfWork.SubCategories.GetItemsByCategoryId(catId),
                        Teams = unitOfWork.Teams.GetItemsBySubCategoryId(subId),
                        SelectedCategory = new AdminMenuItemViewModel { Id = catId },
                        SelectedSubCategory = new AdminMenuItemViewModel { Id = subId },
                        SelectedTeam = new AdminMenuItemViewModel { NewName = teamName }
                    },
                    "Information architecture",
                    Array.Empty<byte>())
                {
                    Menu = this.unitOfWork.AdminMenu.GetItems().ToList(),
                    Languages = this.unitOfWork.Languages.GetItems().ToList()
                });

            return Task.CompletedTask;
        }
    }
}
