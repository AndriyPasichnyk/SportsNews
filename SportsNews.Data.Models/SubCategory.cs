﻿using System.Collections.Generic;

namespace SportsNews.Data.Models
{
    public class SubCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsVisible { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public IEnumerable<Team> Teams { get; set; }
    }
}
