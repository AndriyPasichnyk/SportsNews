﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;

namespace SportsNews
{
    public static class HTMLUserHelper
    {
        public static HtmlString GetUserFullNameFromClaims(this IHtmlHelper html, ClaimsPrincipal user)
        {
            var claimFN = user.Claims.FirstOrDefault(x => x.Type == Claims.FirstName);
            string firstName = claimFN != null ? claimFN.Value : string.Empty;

            var claimLN = user.Claims.FirstOrDefault(x => x.Type == Claims.LastName);
            string lastName = claimLN != null ? claimLN.Value : string.Empty;

            return new HtmlString($"{firstName} {lastName}");
        }
    }
}
