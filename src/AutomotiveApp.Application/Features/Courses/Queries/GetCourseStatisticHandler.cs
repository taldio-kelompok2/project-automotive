using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Enums;
using AutomotiveApp.WebAPI.Dto.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public class GetCourseStatisticHandler(IUnitOfWork uow)
    : IRequestHandler<GetCourseStatistic, IEnumerable<CourseStatisticDto>>
    {
        public async Task<IEnumerable<CourseStatisticDto>> Handle(GetCourseStatistic request, CancellationToken ct)
        {
            var query = uow.OrderItemRepo.Query()
                .Where(oi => oi.Order.Status == OrderStatus.Finished)
                .Select(oi => new
                {
                    oi.Price,
                    oi.OrderId,
                    oi.Order.UserId,
                    oi.Session.Course.Id,
                    oi.Session.Course.Name,
                    Category = oi.Session.Course.Category != null ? oi.Session.Course.Category.Name : null,
                    oi.Order.CreatedAt
                });

            if (request.Year != null)
                query = query.Where(x => x.CreatedAt.Year == request.Year.Value);

            var baseStats = await query
                .GroupBy(x => new { x.Id, x.Name, x.Category })
                .Select(g => new CourseStatisticDto
                {
                    CourseId = g.Key.Id,
                    CourseTitle = g.Key.Name,
                    Category = g.Key.Category,

                    TotalOrders = g.Select(x => x.OrderId).Distinct().Count(),
                    TotalStudents = g.Select(x => x.UserId).Distinct().Count(),
                    TotalRevenue = g.Sum(x => x.Price),

                    MonthlySales = g.GroupBy(x => new { x.CreatedAt.Year, x.CreatedAt.Month })
                        .Select(m => new MonthlySalesDto
                        {
                            Year = m.Key.Year,
                            Month = m.Key.Month,
                            TotalOrders = m.Select(x => x.OrderId).Distinct().Count(),
                            TotalRevenue = m.Sum(x => x.Price)
                        })
                        .OrderBy(m => m.Year).ThenBy(m => m.Month)
                        .ToList(),

                    DailySales = g.GroupBy(x => x.CreatedAt.Date)
                        .Select(d => new DailySalesDto
                        {
                            Date = d.Key,
                            TotalOrders = d.Select(x => x.OrderId).Distinct().Count(),
                            TotalRevenue = d.Sum(x => x.Price)
                        })
                        .OrderBy(d => d.Date)
                        .ToList(),
                }).ToListAsync();

            return baseStats;
        }
    }
}