using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Infrastructure.Repositories;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class OrderRepository(AppDbContext context)
    : BaseRepository<Order>(context), IOrderRepository
    {

    }
}