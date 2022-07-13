using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SportsNews.Data;
using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Components
{
    public class AdminContentMenuViewComponent : ViewComponent
    {
        private readonly IUnitOfWork unitOfWork;
        public AdminContentMenuViewComponent(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menuItems = this.unitOfWork.Categories.GetItems().ToList();
            return View(menuItems);
        }
    }
}
