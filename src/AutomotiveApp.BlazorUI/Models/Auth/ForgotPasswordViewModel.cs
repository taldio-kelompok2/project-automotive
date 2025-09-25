using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.BlazorUI.Models.Auth
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "email is invalid")]
        public string Email { get; set; } = null!;
        public bool Success = false;
    }
}