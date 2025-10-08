using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Commands
{
    public class EditCourseCategoryCommandHandler(IUnitOfWork _uow, IMapper _mapper)
    : IRequestHandler<EditCourseCategoryCommand, CourseCategoryQueryDto>
    {
        public async Task<CourseCategoryQueryDto> Handle(EditCourseCategoryCommand request, CancellationToken ct)
        {

            var oldData = await _uow.CourseCategoryRepo.GetByIdAsync(request.NewData.Id, ct: ct)
            ?? throw new NotFoundException<CourseCategory>(request.NewData.Id);

            _mapper.Map(request.NewData, oldData);
            _uow.CourseCategoryRepo.Update(oldData);

            var saved = await _uow.SaveChangesAsync(ct);
            if (saved > 0) return _mapper.Map<CourseCategoryQueryDto>(oldData);
            else throw new InvalidOperationException("Unable to save the updated course category data, please try again.");

        }
    }
}