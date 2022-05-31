using System.ComponentModel.DataAnnotations;

namespace SportsNews.Models
{
    public class UserInfoViewModel
    {
        public UserInfoViewModel()
        { }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}