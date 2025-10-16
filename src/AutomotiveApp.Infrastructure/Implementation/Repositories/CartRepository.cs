using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Infrastructure.Repositories;
using AutomotiveApp.Shared.Exceptions;
using MediatR;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class CartRepository(AppDbContext context)
    : BaseRepository<Cart>(context), ICartRepository
    {

        public Task<Unit> BatchDelete(IEnumerable<CartItem> items, CancellationToken ct = default)
        {
            if (!items.Any())
                throw new NotFoundException<Cart>("No items found for the provided Ids in cart.");

            _context.CartItems.RemoveRange(items);
            return Task.FromResult(Unit.Value);
        }
    }
}