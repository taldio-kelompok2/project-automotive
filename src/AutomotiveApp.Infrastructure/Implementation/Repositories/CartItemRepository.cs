using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class CartItemRepository(AppDbContext context)
    : BaseRepository<CartItem>(context), ICartItemRepository
    {
        public async Task<int> RecalculateCartTotalAsync(Guid id)
        {
            var totalPrice = await Query()
            .Where(ci => ci.CartId == id)
            .Select(ci => (int?)ci.Session.Course.Price)
            .SumAsync() ?? 0;

            return totalPrice;
        }
    }
}