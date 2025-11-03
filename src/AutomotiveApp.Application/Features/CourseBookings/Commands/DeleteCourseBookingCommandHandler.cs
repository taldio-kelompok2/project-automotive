using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseBookings.Commands
{
    public class DeleteCourseBookingCommandHandler(IUnitOfWork _uow)
: IRequestHandler<DeleteCourseBookingCommand>
    {
        public async Task<Unit> Handle(DeleteCourseBookingCommand request, CancellationToken cancellationToken)
        {
            var data = await _uow.CourseBookingRepo.GetByIdAsync(request.Id, ct: cancellationToken)
            ?? throw new NotFoundException<CourseBooking>($"Course with ID {request.Id} was not found.");

            _uow.CourseBookingRepo.Delete(data);
            var saved = await _uow.SaveChangesAsync(cancellationToken);
            if (saved > 0) return Unit.Value;

            throw new InvalidOperationException("Unable to delete the course Booking, Please try again.");

        }
    }
}
