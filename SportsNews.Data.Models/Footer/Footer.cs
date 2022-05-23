using System;
using System.Collections.Generic;
using System.Text;

namespace SportsNews.Data.Models
{
    public class Footer
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public string Name { get; set; }

        public bool IsShown { get; set; }
    }
}
