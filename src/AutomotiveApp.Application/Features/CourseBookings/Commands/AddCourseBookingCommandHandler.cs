using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.CourseBookings.Commands
{
    public class AddCourseBookingsHandler(IUnitOfWork uow, IMapper mapper, UserManager<User> userManager)
    : IRequestHandler<AddCourseBookingCommand, CourseBookingQueryDto>
    {
        public async Task<CourseBookingQueryDto> Handle(AddCourseBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Data.UserId.ToString())
                ?? throw new KeyNotFoundException($"User with Id {request.Data.UserId} not found.");

            var session = await uow.CourseSessionRepo.FirstOrDefaultAsync(
                modifier: q => q.Include(s => s.Course),
                predicate: s => s.Id == request.Data.SessionId,
                ct: cancellationToken
            ) ?? throw new KeyNotFoundException($"Course session with Id {request.Data.SessionId} not found.");

            var mappedItem = mapper.Map<CourseBooking>(request.Data);

            mappedItem.User = user;
            mappedItem.Session = session;

            await uow.CourseBookingRepo.AddAsync(mappedItem);

            var saved = await uow.SaveChangesAsync(cancellationToken);

            if (saved > 0)
            {
                return mapper.Map<CourseBookingQueryDto>(mappedItem);
            }
            else
            {
                throw new InvalidOperationException("Unable to save the new course booking. Please try again.");
            }
        }
    }
}