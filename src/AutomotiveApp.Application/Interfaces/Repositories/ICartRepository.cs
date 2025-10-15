using AutomotiveApp.Domain.Entities.Courses.Cart;

namespace AutomotiveApp.Application.Interfaces.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task BatchDelete(IEnumerable<CartItem> items, CancellationToken ct = default);
    }
}