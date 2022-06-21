using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportsNews.Data.Models;
using System.Collections.Generic;

namespace SportsNews.Helpers
{
    public static class HTMLListHelper
    {
        public static HtmlString CreateCategoryList(this IHtmlHelper html, IEnumerable<Category> listItems)
        {
            string result = $"<div class=\"col-4\" style=\"max-width: 260px; text-align: left\">";
            result = $"{result}<div class=\"list-group\">";
            foreach (Category item in listItems)
            {
                result = $"{result}<a class=\"list-group-item list-group-item-action\" asp-controller=\"Admin\" asp-action=\"InfoArchitecture\" asp-route-id=\"{item.Id}\">{item.Name}</a>";
            }
            result = $"{result}</div></div>";
            return new HtmlString(result);
        }
    }
}
