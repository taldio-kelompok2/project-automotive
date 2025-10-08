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
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;


        public async Task<IEnumerable<CourseQueryDto>> Handle(GetCourses request, CancellationToken ct)
        {
            static IQueryable<Course> modifier(IQueryable<Course> q) => q.Include(c => c.Category);

            try
            {
                var items = await _uow.CourseRepo.GetAllAsync(modifier, ct);

                var mappedItems = _mapper.Map<IEnumerable<CourseQueryDto>>(items);
                return mappedItems;
            }
            catch
            {
                throw;
            }
        }
    }
}