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
        public bool IsAdminMode { get; set; }

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
            IsAdminMode = false;
            UserImg = null;
        }

        public LayoutViewModel(string title, bool isAdminMode, byte[] userImg)
        {
            Title = title;
            IsAdminMode = isAdminMode;
            UserImg = userImg;
        }
    }

    public class LayoutViewModel<T> : LayoutViewModel
    {
        public T PageModel { get; set; }

        public LayoutViewModel() : base(string.Empty, false, null)
        { }

        public LayoutViewModel(T pageModel, string title) : base(title)
        {
            PageModel = pageModel;
        }

        public LayoutViewModel(T pageModel, string title, bool isAdminMode) : base(title, isAdminMode, null)
        {
            PageModel = pageModel;
        }

        public LayoutViewModel(T pageModel, string title, bool isAdminMode, byte[] userImg) : base(title, isAdminMode, userImg)
        {
            PageModel = pageModel;
        }
    }
}
