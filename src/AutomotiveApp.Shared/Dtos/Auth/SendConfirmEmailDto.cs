using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.Auth
{
    public class SendConfirmEmailDto
    {
        [Required]//err
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
