using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public class GetDashboardHandler(UserManager<User> userManager, IInvoiceRepository invoiceRepository, ICourseRepository courseRepo) 
        : IRequestHandler<GetDashboard, DashboardDto>
    {
        public async Task<DashboardDto> Handle(GetDashboard request, CancellationToken cancellationToken)
        {
            var totalUsers = await userManager.Users.CountAsync(cancellationToken);

            var activeThreshold = DateTime.UtcNow.AddMinutes(-15); // ganti ke jwtsettings?
            var activeUsers = await userManager.Users
                .CountAsync(u => u.LastLogin >= activeThreshold, cancellationToken);

            var totalRevenue = await invoiceRepository.GetTotalRevenue();

            return new DashboardDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                TotalRevenue = totalRevenue
            };
        }
    }
}
