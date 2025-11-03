using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.CourseBookings.Commands
{
    public class EditCourseBookingCourseCommandHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<EditCourseBookingCourseCommand, CourseBookingQueryDto>
    {
        public async Task<CourseBookingQueryDto> Handle(EditCourseBookingCourseCommand request, CancellationToken cancellationToken)
        {
            var booking = await uow.CourseBookingRepo.FirstOrDefaultAsync(
            modifier: q => q
                .Include(b => b.User)
                .Include(b => b.Session)
                    .ThenInclude(s => s.Course),
            predicate: b => b.Id == request.BookingId,
            ct: cancellationToken)
            ?? throw new NotFoundException<CourseBooking>(request.BookingId);

            var newSession = await uow.CourseSessionRepo.FirstOrDefaultAsync(
            modifier: q => q.Include(s => s.Course),
            predicate: s => s.Id == request.NewSessionId,
            ct: cancellationToken)

            ?? throw new NotFoundException<CourseSession>(request.NewSessionId);
            booking.SessionId = newSession.Id;
            booking.Session = newSession;

            uow.CourseBookingRepo.Update(booking);
            var saved = await uow.SaveChangesAsync(cancellationToken);
            if (saved == 0)
                throw new InvalidOperationException("Unable to update the course booking. Please try again.");

            // Return DTO
            return mapper.Map<CourseBookingQueryDto>(booking);
        }
    }

}