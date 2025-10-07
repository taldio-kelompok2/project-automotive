using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Commands
{
    public class AddCourseCommandHandler(IUnitOfWork _uow, IMapper _mapper)
    : IRequestHandler<AddCourseCommand, CourseQueryDto>
    {
        public async Task<CourseQueryDto> Handle(AddCourseCommand request, CancellationToken ct)
        {
            var mappedItems = _mapper.Map<Course>(request.Data);

            var category = await _uow.CourseCategoryRepo.GetByIdAsync(request.Data.CategoryId, ct: ct)
            ?? throw new KeyNotFoundException("Course category not found.");

            mappedItems.Category = category;

            await _uow.CourseRepo.AddAsync(mappedItems);
            var saved = await _uow.SaveChangesAsync(ct);
            if (saved > 0) return _mapper.Map<CourseQueryDto>(mappedItems);
            else throw new InvalidOperationException("Unable to save the new course data please try again.");

        }
    }
}