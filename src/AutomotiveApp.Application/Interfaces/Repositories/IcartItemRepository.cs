using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Domain.Entities.Courses.Cart;

namespace AutomotiveApp.Application.Interfaces.Repositories
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        public Task<int> RecalculateCartTotalAsync(Guid id);
    }
}