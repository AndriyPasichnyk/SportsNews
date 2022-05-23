using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Controllers
{
    public class NavBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var aaa = new List<string>() { "View profile", "Change Password", "My surveys", "Team hub" };
            var model = new UserMenuView { Names = aaa };
            return View(model);
        }
    }

    public class UserMenuView
    {
        public List<string> Names { get; set; }
    }

}
