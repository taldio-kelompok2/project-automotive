using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Commands
{
    public class AddCourseCommandHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<AddCourseCommand, CourseQueryDto>
    {
        public async Task<CourseQueryDto> Handle(AddCourseCommand request, CancellationToken cancellationToken)
        {
            var mappedItem = mapper.Map<Course>(request.Data);

            var category = await uow.CourseCategoryRepo.GetByIdAsync(request.Data.CategoryId, ct: cancellationToken)
            ?? throw new KeyNotFoundException("Course category not found.");

            mappedItem.Category = category;
            await uow.CourseRepo.AddAsync(mappedItem);
            var saved = await uow.SaveChangesAsync(cancellationToken);
            if (saved > 0) return mapper.Map<CourseQueryDto>(mappedItem);
            else throw new InvalidOperationException("Unable to save the new course data please try again.");

        }
    }
}