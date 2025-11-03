using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public class GetCoursesHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCourses, IEnumerable<CourseQueryDto>>
    {
        public async Task<IEnumerable<CourseQueryDto>> Handle(GetCourses request, CancellationToken cancellationToken)
        {

            try
            {
                var items = await uow.CourseRepo.GetCoursesWithCategory(cancellationToken);
                var mappedItems = mapper.Map<IEnumerable<CourseQueryDto>>(items);
                return mappedItems;
            }
            catch
            {
                throw;
            }
        }
    }
}