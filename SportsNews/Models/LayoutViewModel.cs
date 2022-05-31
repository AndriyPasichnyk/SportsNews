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

        public LayoutViewModel(string title)
        {
            Title = title;
            IsAdminMode = false;
        }

        public LayoutViewModel(string title, bool isAdminMode)
        {
            Title = title;
            IsAdminMode = isAdminMode;
        }
    }

    public class LayoutViewModel<T> : LayoutViewModel
    {
        public T PageModel { get; set; }

        public LayoutViewModel() : base(string.Empty, false)
        { }

        public LayoutViewModel(T pageModel, string title) : base(title, false)
        {
            PageModel = pageModel;
        }

        public LayoutViewModel(T pageModel, string title, bool isAdminMode) : base(title, isAdminMode)
        {
            PageModel = pageModel;
        }

    }
}
