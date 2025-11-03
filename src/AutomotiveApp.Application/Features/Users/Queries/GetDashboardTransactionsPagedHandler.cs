using AutoMapper;
using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Shared.Dtos.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public class GetDashboardTransactionsPagedHandler(IInvoiceRepository invoiceRepository, IOrderItemRepository orderItemRepository, IMapper mapper) 
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
                itemTaken: request.PageSize,
                ct: cancellationToken);
            var invoices = paged.Items;

            var result = new List<DashboardTransactionDto>();

            foreach (var inv in invoices)
            {
                var dto = mapper.Map<DashboardTransactionDto>(inv);
                dto.CourseCount = await orderItemRepository.CountAsync(predicate: oi => oi.OrderId == inv.OrderId, ct: cancellationToken);
                result.Add(dto);
            }

            return result;
        }
    }
}
