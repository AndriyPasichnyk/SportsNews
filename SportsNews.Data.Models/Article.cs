using System;
using System.Collections.Generic;
using System.Text;

namespace SportsNews.Data.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        //public int CategoryId { get; set; }
        //public Category Category { get; set; }

        //public int SubCategoryId { get; set; }
        //public SubCategory SubCategory { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
