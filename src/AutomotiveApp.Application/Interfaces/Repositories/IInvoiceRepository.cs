using AutomotiveApp.Domain.Entities.Invoices;

namespace AutomotiveApp.Application.Interfaces.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<int> GetLastInvoiceNumber();
        Task<long> GetTotalRevenue();
    }
}