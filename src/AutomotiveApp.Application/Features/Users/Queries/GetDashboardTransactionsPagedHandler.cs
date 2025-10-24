using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Shared.Dtos.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public class GetDashboardTransactionsPagedHandler(IInvoiceRepository invoiceRepository, IOrderItemRepository orderItemRepository) 
        : IRequestHandler<GetDashboardTransactionsPaged, List<DashboardTransactionDto>>
    {
        public async Task<List<DashboardTransactionDto>> Handle(GetDashboardTransactionsPaged request, CancellationToken cancellationToken)
        {
            var paged = await invoiceRepository.GetAllPagedAsync(
                modifier: i => i
                .Include(i => i.Order)
                    .ThenInclude(o => o.User)
                .Include(i => i.Order.PaymentMethod)
                .OrderByDescending(i => i.CreatedAt),
                page: request.Page,
                itemTaken: request.PageSize);
            var invoices = paged.Items;

            var result = new List<DashboardTransactionDto>();

            foreach (var inv in invoices)
            {
                result.Add(new DashboardTransactionDto
                {
                    Email = inv.Order.User.Email,
                    UserName = inv.Order.User.UserName,
                    CourseCount = await orderItemRepository.CountAsync(predicate: oi => oi.OrderId == inv.OrderId),
                    CreatedAt = inv.CreatedAt,
                    InvoiceCode = inv.InvoiceCode,
                    TotalPrice = inv.TotalPrice,
                    PaymentMethodName = inv.Order.PaymentMethod.Name,
                });
            }

            return result;
        }
    }
}
