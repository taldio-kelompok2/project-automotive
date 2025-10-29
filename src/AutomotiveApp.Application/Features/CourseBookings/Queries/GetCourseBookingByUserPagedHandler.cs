using System.Linq.Expressions;
using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Exceptions;
using AutomotiveApp.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.CourseBookings.Queries
{
    public class GetCourseBookingByUserPagedHandler(IUnitOfWork Uow, IMapper Mapper)
    : IRequestHandler<GetCourseBookingByUserPaged, PaginatedResult<CourseBookingQueryDto>>
    {
        public async Task<PaginatedResult<CourseBookingQueryDto>> Handle(GetCourseBookingByUserPaged request, CancellationToken ct)
        {
            static IQueryable<CourseBooking> modifier(IQueryable<CourseBooking> q) =>
                q.Include(cb => cb.User)
                .Include(cb => cb.Session)
                .ThenInclude(s => s.Course)
                .ThenInclude(c => c.Category);

            Expression<Func<CourseBooking, bool>> predicate = cb => cb.UserId == request.UserId;

            if (request.CourseId != null)
                predicate = cb => cb.UserId == request.UserId && cb.Session.CourseId == request.CourseId;

            var (items, total) = await Uow.CourseBookingRepo.FindPagedAsync(predicate, modifier, request.Page, request.ItemTaken, false, ct);

            var mappedItems = Mapper.Map<IEnumerable<CourseBookingQueryDto>>(items);
            return new PaginatedResult<CourseBookingQueryDto>(mappedItems, total);
        }
    }

}
