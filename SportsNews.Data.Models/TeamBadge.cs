using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportsNews.Data.Models
{
    public class TeamBadge
    {
        [Key]
        public int Id { get; set; }

        public byte[] Badge { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; }
    }
}

