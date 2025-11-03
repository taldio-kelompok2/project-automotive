using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Commands
{
    public class AddCourseCategoryCommandHandler(IUnitOfWork _uow, IMapper _mapper)
    : IRequestHandler<AddCourseCategoryCommand, CourseCategoryQueryDto>
    {
        public async Task<CourseCategoryQueryDto> Handle(AddCourseCategoryCommand request, CancellationToken cancellationToken)
        {
            var mappedItem = _mapper.Map<CourseCategory>(request.Data);

            await _uow.CourseCategoryRepo.AddAsync(mappedItem);
            var saved = await _uow.SaveChangesAsync(cancellationToken);
            if (saved > 0) return _mapper.Map<CourseCategoryQueryDto>(mappedItem);

            else throw new InvalidOperationException("Unable to save the new course category data please try again.");

        }
    }
}