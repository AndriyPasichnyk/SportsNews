using System.ComponentModel.DataAnnotations;

namespace SportsNews.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The E-mail is required for login.")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "The E-mail field is not a valid e-mail address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password is required for login.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
