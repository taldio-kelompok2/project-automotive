using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public class GetCourseByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCourseById, CourseQueryDetailDto>
    {
        public async Task<CourseQueryDetailDto> Handle(GetCourseById request, CancellationToken ct)
        {
            try
            {
                var item = await uow.CourseRepo.GetCourseDetailById(request.Id, ct);

                return mapper.Map<CourseQueryDetailDto>(item);
            }
            catch
            {
                throw;
            }
        }
    }
}