using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public static class LocalizationInitializer
    {
        public static void SetSupportedCultures(IOptions<RequestLocalizationOptions> options, IUnitOfWork unitOfWork)
        {
            var cultureList = options.Value.SupportedCultures.ToList();
            foreach (var item in unitOfWork.Languages.GetItems())
            {
                var culture = new CultureInfo(item.Abbreviation);
                if (!options.Value.SupportedCultures.Contains(culture))
                {
                    cultureList.Add(culture);
                }
            }
            options.Value.SupportedCultures = cultureList;
            options.Value.SupportedUICultures = cultureList;
        }
    }
}
