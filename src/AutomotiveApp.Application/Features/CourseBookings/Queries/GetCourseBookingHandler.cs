using AutoMapper;
using AutomotiveApp.Application.Features.Courses.Queries;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.CourseBookings.Queries
{
    public class GetCourseBookingsHandler(IUnitOfWork Uow, IMapper Mapper)
    : IRequestHandler<GetCourseBookings, IEnumerable<CourseBookingQueryDto>>
    {
        public async Task<IEnumerable<CourseBookingQueryDto>> Handle(GetCourseBookings request, CancellationToken ct)
        {
            IQueryable<CourseBooking> modifier(IQueryable<CourseBooking> q) =>
                q.Include(cb => cb.User)
                .Include(cb => cb.Session)
                    .ThenInclude(s => s.Course)
                .Where(cb => cb.SessionId == request.SessionId || request.SessionId == null);

            var items = await Uow.CourseBookingRepo.GetAllAsync(modifier: modifier, ct: ct);
            var mappedItems = Mapper.Map<IEnumerable<CourseBookingQueryDto>>(items).ToList();

            return mappedItems;
        }
    }

}