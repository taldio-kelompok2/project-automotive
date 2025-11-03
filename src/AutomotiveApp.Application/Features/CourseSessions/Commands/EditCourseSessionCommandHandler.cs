using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.CourseSessions.Commands
{
    public class EditCourseSessionCommandHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<EditCourseSessionCommand, CourseSessionQueryDto>
    {
        public async Task<CourseSessionQueryDto> Handle(EditCourseSessionCommand request, CancellationToken cancellationToken)
        {
            static IQueryable<CourseSession> modifier(IQueryable<CourseSession> q) => q.Include(c => c.Course);

            var oldData = await uow.CourseSessionRepo.GetByIdAsync(request.NewData.Id, modifier, ct: cancellationToken)
            ?? throw new NotFoundException<CourseSession>(request.NewData.Id);

            var course = await uow.CourseRepo.GetByIdAsync(request.NewData.CourseId, ct: cancellationToken)
            ?? throw new NotFoundException<Course>(request.NewData.CourseId);
            oldData.Course = course;

            // update the old Data
            mapper.Map(request.NewData, oldData);
            uow.CourseSessionRepo.Update(oldData);

            var saved = await uow.SaveChangesAsync(cancellationToken);
            if (saved > 0) return mapper.Map<CourseSessionQueryDto>(oldData);
            else throw new InvalidOperationException("Unable to save the updated course Session data, please try again.");
        }
    }
}