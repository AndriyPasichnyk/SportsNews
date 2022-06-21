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

        public IEnumerable<TeamLocation> TeamLocation { get; set; }

        public DateTime DateAdded { get; set; }

        public TeamBadge TeamBadge { get; set; }
    }
}
