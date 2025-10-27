using System.Linq.Expressions;
using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.CourseBookings.Queries
{
    public class GetCourseBookingByUserHandler(IUnitOfWork Uow, IMapper Mapper)
    : IRequestHandler<GetCourseBookingByUser, IEnumerable<CourseBookingQueryDto>>
    {
        public async Task<IEnumerable<CourseBookingQueryDto>> Handle(GetCourseBookingByUser request, CancellationToken ct)
        {
            static IQueryable<CourseBooking> modifier(IQueryable<CourseBooking> q) =>
                q.Include(cb => cb.User)
                .Include(cb => cb.Session)
                .ThenInclude(s => s.Course)
                .ThenInclude(c => c.Category);

            Expression<Func<CourseBooking, bool>> predicate = cb => cb.UserId == request.UserId;

            if (request.CourseId != null)
                predicate = cb => cb.UserId == request.UserId && cb.Session.CourseId == request.CourseId;

            var items = await Uow.CourseBookingRepo.FindAsync(predicate, modifier, ct);

            var mappedItems = Mapper.Map<IEnumerable<CourseBookingQueryDto>>(items).ToList();
            return mappedItems;
        }
    }

}
