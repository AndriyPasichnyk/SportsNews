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

        [StringLength(300)]
        public string FullName { get; set; }

        public IEnumerable<Team> Team { get; set; }
    }
}
