using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportsNews.Data.Models
{
    public class Location
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Country { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        [StringLength(255)]
        public string FullName { get; set; }

        public IEnumerable<TeamLocation> TeamLocation { get; set; }
    }

    public class TeamLocation
    {
        [Key]
        public int Id { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
