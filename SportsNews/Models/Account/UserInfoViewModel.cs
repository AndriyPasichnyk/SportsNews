using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

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

        public byte[] Image { get; set; }

        [NotMapped]
        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
    }
}