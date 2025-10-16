using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Invoices;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class InvoiceRepository(AppDbContext context)
    : BaseRepository<Invoice>(context), IInvoiceRepository
    {
        public async Task<int> GetLastInvoiceNumber()
        {
            return await Query().Select(i => (int?)i.InvoiceNumber).MaxAsync() ?? 0;
        }
    }
}