using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AutomotiveApp.Application.Features.CourseSessions.Commands
{
    public record AddCourseSessionCommandHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<AddCourseSessionCommand, CourseSessionQueryDto>
    {
        public async Task<CourseSessionQueryDto> Handle(AddCourseSessionCommand request, CancellationToken ct)
        {
            var mappedItem = mapper.Map<CourseSession>(request.Data);

            var course = await uow.CourseRepo.GetByIdAsync(mappedItem.CourseId)
            ?? throw new NotFoundException<Course>(mappedItem.CourseId);

            mappedItem.Course = course;
            await uow.CourseSessionRepo.AddAsync(mappedItem);
            var saved = await uow.SaveChangesAsync(ct);

            if (saved > 0) return mapper.Map<CourseSessionQueryDto>(mappedItem);
            else throw new InvalidOperationException("Unable to save the new course Session data please try again.");


        }
    }
}