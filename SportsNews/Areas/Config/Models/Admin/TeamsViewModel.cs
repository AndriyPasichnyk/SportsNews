using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Models
{
    public class TeamsViewModel
    {
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<SubCategory> SubCategories { get; set; }

        public IEnumerable<Team> Teams { get; set; }

        public AdminMenuItemViewModel SelectedCategory { get; set; }

        public AdminMenuItemViewModel SelectedSubCategory { get; set; }

        public AdminMenuItemViewModel SelectedTeam { get; set; }
    }
}
