using System.ComponentModel.DataAnnotations;

namespace SportsNews.Data.Models
{
    public class UserPhoto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; }

        public string ProfilePicture { get; set; }
    }
}