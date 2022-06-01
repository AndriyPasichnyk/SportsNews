using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Models
{
    public class LayoutViewModel
    {
        public string Title { get; set; }
        public bool IsAdminMode { get; set; }

        public string UserImg { get; set; }

        public LayoutViewModel(string title)
        {
            Title = title;
            IsAdminMode = false;
            UserImg = string.Empty;
        }

        public LayoutViewModel(string title, bool isAdminMode, string userImg)
        {
            Title = title;
            IsAdminMode = isAdminMode;
            UserImg = userImg;
        }
    }

    public class LayoutViewModel<T> : LayoutViewModel
    {
        public T PageModel { get; set; }

        public LayoutViewModel() : base(string.Empty, false, string.Empty)
        { }

        public LayoutViewModel(T pageModel, string title) : base(title)
        {
            PageModel = pageModel;
        }

        public LayoutViewModel(T pageModel, string title, bool isAdminMode) : base(title, isAdminMode, string.Empty)
        {
            PageModel = pageModel;
        }

        public LayoutViewModel(T pageModel, string title, bool isAdminMode, string userImg) : base(title, isAdminMode, userImg)
        {
            PageModel = pageModel;
        }
    }
}
