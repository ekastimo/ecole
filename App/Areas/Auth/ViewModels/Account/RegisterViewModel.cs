using System;
using System.ComponentModel.DataAnnotations;
using App.Areas.Crm.Enums;

namespace App.Areas.Auth.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }
         
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        [Phone]
        [Required]
        public string Phone { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
