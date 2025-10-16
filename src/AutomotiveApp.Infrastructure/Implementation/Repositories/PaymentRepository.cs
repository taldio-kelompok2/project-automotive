using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Domain.Entities.Payments;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Infrastructure.Repositories;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class PaymentRepository(AppDbContext context)
    : BaseRepository<PaymentMethod>(context), IPaymentRepository
    {

    }
}