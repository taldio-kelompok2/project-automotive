using AutomotiveApp.Domain.Entities.Courses.Cart;
using MediatR;

namespace AutomotiveApp.Application.Interfaces.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        void BatchDelete(IEnumerable<CartItem> items, CancellationToken ct = default);
    }
}