using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.Carts
{
    public class CartCreateDto
    {
        [Required(ErrorMessage = "UserId wajib diisi.")]
        public Guid UserId { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "TotalPrice minimal 0.")]
        public long TotalPrice { get; set; } = 0;
    }
}
