using System.ComponentModel.DataAnnotations;

namespace App.Areas.Auth.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
