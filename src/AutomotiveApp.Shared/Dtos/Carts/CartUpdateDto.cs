using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.Carts
{
    public class CartUpdateDto
    {
        [Range(0, long.MaxValue, ErrorMessage = "TotalPrice minimal 0.")]
        public long TotalPrice { get; set; }
        
        [Required(ErrorMessage = "UserId wajib diisi.")]
        public Guid UserId { get; set; }
    }
}
