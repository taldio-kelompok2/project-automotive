using AutomotiveApp.BlazorUI.Models.Cart;
using AutomotiveApp.BlazorUI.Models.Course;
using AutomotiveApp.Shared.Dtos.CartItems;
using AutomotiveApp.Shared.Dtos.Carts;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface ICartService
    {
        Task<ApiResponse<CartReadDetailsDto>> CreateAsync(Guid userId, CancellationToken ct = default);

        Task<ApiResponse<CartReadDetailsDto>> GetUserCart(CancellationToken ct = default);

        Task<ApiResponse<CartItemReadDto>> AddItemAsync(CartItemViewModel vm, CancellationToken ct = default);

        Task<ApiResponse<string>> BatchDeleteItemsAsync(List<Guid> Ids, CancellationToken ct = default);

        // Task<bool> UpdateAsync(Guid id, UpdateCourseViewModel vm, CancellationToken ct = default);
    }
}