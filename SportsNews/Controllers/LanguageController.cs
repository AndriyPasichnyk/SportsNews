using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using SportsNews.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SportsNews.Controllers
{
    public class LanguageController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public LanguageController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("/Language/Change/{id}")]
        public IActionResult Change(int id)
        {
            string referer = HttpContext.Request.Headers["Referer"].ToString();

            var culture = unitOfWork.Languages.GetItemByID(id);

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture.Abbreviation.ToLower())),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
