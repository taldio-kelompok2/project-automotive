using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.Auth
{
    public class ConfirmEmailDto
    {
        [Required]//err
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
