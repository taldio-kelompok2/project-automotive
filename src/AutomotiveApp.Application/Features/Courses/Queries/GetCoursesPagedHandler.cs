using AutoMapper;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Domain.Interface;
using AutomotiveApp.Shared.Dtos.Course;
using AutomotiveApp.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public class GetCoursesPagedHandler(IRepository<Course> repo, IMapper mapper)
    : IRequestHandler<GetCoursesPaged, PaginatedResult<CourseQueryDto>>
    {
        private readonly IRepository<Course> _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResult<CourseQueryDto>> Handle(GetCoursesPaged request, CancellationToken ct)
        {
            static IQueryable<Course> modifier(IQueryable<Course> q) => q.Include(c => c.Category);

            try
            {
                var (items, total) = await _repo.GetAllPagedAsync(page: request.Page, itemTaken: request.ItemTaken, ct: ct, modifier: modifier);

                var mappedItems = _mapper.Map<IEnumerable<CourseQueryDto>>(items);
                return new PaginatedResult<CourseQueryDto>(mappedItems, total);
            }
            catch
            {
                throw;
            }
        }
    }
}