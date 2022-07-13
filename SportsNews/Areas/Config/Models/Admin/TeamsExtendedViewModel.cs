using Microsoft.AspNetCore.Http;
using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Models
{
    public class TeamsExViewModel : TeamsViewModel
    {
        public bool Edit { get; set; }

        public byte[] Image { get; set; }

        [NotMapped]
        [Display(Name = "Profile Picture")]
        public IFormFile BadgeImage { get; set; }

        public Location Location { get; set; }
    }
}
