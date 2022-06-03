using System;
using System.ComponentModel.DataAnnotations;

namespace SportsNews.Data.Models
{
    public class UserPhoto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public Guid UserId { get; set; }

        public byte[] ProfilePicture { get; set; }
    }
}