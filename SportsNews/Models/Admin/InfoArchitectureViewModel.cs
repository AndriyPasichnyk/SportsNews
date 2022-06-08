using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Models
{
    public class InfoArchitectureViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public int SelectedCategoryId { get; set; }

        public IEnumerable<SubCategory> SubCategories { get; set; }
        public int SelectedSubCategoryId { get; set; }

        public IEnumerable<Team> Teams { get; set; }

        public string Name { get; set; }
    }
}
