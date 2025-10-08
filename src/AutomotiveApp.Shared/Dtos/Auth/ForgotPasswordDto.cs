using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.Auth
{
    public class ForgotPasswordDto
    {
        [Required]//err
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
