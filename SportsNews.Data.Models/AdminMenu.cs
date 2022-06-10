using System;
using System.Collections.Generic;
using System.Text;

namespace SportsNews.Data.Models
{
    public class AdminMenu
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Action { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}
