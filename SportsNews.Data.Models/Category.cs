using System.Collections.Generic;

namespace SportsNews.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsVisible { get; set; }

        public bool IsStatic { get; set; }

        public IEnumerable<SubCategory> Subcategories { get; set; }
    }
}
