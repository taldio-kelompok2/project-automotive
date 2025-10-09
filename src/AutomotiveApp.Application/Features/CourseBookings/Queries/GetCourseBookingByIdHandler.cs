using AutoMapper;
using AutomotiveApp.Application.Features.Courses.Queries;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.CourseBookings.Queries
{
    public class GetCourseBookingByIdHandler(IUnitOfWork Uow, IMapper Mapper)
    : IRequestHandler<GetCourseBookingById, CourseBookingQueryDto>
    {
        public async Task<CourseBookingQueryDto> Handle(GetCourseBookingById request, CancellationToken ct)
        {
            static IQueryable<CourseBooking> modifier(IQueryable<CourseBooking> q) =>
                q.Include(cb => cb.User)
                    .Include(cb => cb.Session)
                    .ThenInclude(s => s.Course);

            var item = await Uow.CourseBookingRepo.GetByIdAsync(request.Id, modifier);
            var mappedItems = Mapper.Map<CourseBookingQueryDto>(item);

            return mappedItems;
        }
    }

}