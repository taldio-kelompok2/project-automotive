using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.Auth
{
    public class ConfirmEmailRequestDto
    {
        [Required(ErrorMessage = "User Id is required")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Token is required")]
        public string Token { get; set; } = string.Empty;
    }
}
