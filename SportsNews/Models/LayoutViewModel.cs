using Microsoft.AspNetCore.Mvc;
using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Models
{
    public class LayoutViewModel
    {
        public string Title { get; set; }

        public string Language { 
            get
            { return CultureInfo.CurrentCulture.Name.ToUpper(); }
        }
        public IEnumerable<Language> Languages { get; set; }

        public byte[] UserImg { get; set; }

        public IEnumerable<AdminMenu> Menu { get; set; }
        public IEnumerable<Category> UserMenu { get; set; }

        public LayoutViewModel(string title)
        {
            Title = title;
            UserImg = null;
        }

        public LayoutViewModel(string title, byte[] userImg)
        {
            Title = title;
            UserImg = userImg;
        }
    }

    public class LayoutViewModel<T> : LayoutViewModel
    {
        public T PageModel { get; set; }

        public LayoutViewModel() : base(string.Empty, null)
        { }

        public LayoutViewModel(T pageModel, string title) : base(title)
        {
            PageModel = pageModel;
        }

        public LayoutViewModel(T pageModel, string title, byte[] userImg) : base(title, userImg)
        {
            PageModel = pageModel;
        }
    }
}
