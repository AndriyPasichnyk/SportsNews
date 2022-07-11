using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsNews.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        public string Name{ get; set; }

        public bool IsVisible { get; set; }

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; }

        public DateTime DateAdded { get; set; }

        public int TeamBadgeId { get; set; }
        public TeamBadge TeamBadge { get; set; }
    }
}
