using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Models
{
    public class LayoutViewModel
    {
        public string Title { get; set; }

        public LayoutViewModel(string title)
        {
            Title = title;
        }
    }

    public class LayoutViewModel<T> : LayoutViewModel
    {
        public T PageModel { get; set; }

        public LayoutViewModel() : base(string.Empty)
        { }

        public LayoutViewModel(T pageModel, string title) : base(title)
        {
            PageModel = pageModel;
        }
    }
}
