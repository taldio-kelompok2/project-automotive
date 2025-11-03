using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Courses.Commands
{
    public class EditCourseCommandHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<EditCourseCommand, CourseQueryDto>
    {
        public async Task<CourseQueryDto> Handle(EditCourseCommand request, CancellationToken cancellationToken)
        {
            static IQueryable<Course> modifier(IQueryable<Course> q) => q.Include(c => c.Category);

            var oldData = await uow.CourseRepo.GetByIdAsync(request.NewData.Id, modifier, ct: cancellationToken)
            ?? throw new NotFoundException<Course>(request.NewData.Id);

            if (request.NewData.CategoryId.HasValue)
            {
                var category = await uow.CourseCategoryRepo.GetByIdAsync(request.NewData.CategoryId.Value, ct: cancellationToken)
                ?? throw new KeyNotFoundException("Course category not found.");
                oldData.Category = category;
            }

            // update the old Data
            mapper.Map(request.NewData, oldData);
            uow.CourseRepo.Update(oldData);

            var saved = await uow.SaveChangesAsync(cancellationToken);
            if (saved > 0) return mapper.Map<CourseQueryDto>(oldData);
            else throw new InvalidOperationException("Unable to save the updated course data, please try again.");

        }
    }
}