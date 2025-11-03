using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public class GetCourseByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCourseById, CourseQueryDetailDto>
    {
        public async Task<CourseQueryDetailDto> Handle(GetCourseById request, CancellationToken cancellationToken)
        {

            var item = await uow.CourseRepo.GetCourseDetailById(request.Id, cancellationToken)
            ?? throw new NotFoundException<Course>(request.Id);

            if (request.UserId != null)
            {
                var userBookings = await uow.CourseBookingRepo.FindAsync(
                    cb => cb.UserId == request.UserId && cb.Session.CourseId == request.Id,
                    q => q.Include(cb => cb.Session),
                    cancellationToken);

                var bookedDates = userBookings
                    .Select(ub => ub.Session.Date.Date)
                    .ToHashSet();

                item.Sessions = item.Sessions
                    .Where(s => !bookedDates.Contains(s.Date.Date) && s.Date > DateTime.UtcNow)
                    .ToList();
            }

            return mapper.Map<CourseQueryDetailDto>(item);


        }
    }
}