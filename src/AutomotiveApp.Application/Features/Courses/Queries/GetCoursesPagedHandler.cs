using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public class GetCoursesPagedHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCoursesPaged, PaginatedResult<CourseQueryDto>>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResult<CourseQueryDto>> Handle(GetCoursesPaged request, CancellationToken ct)
        {
            static IQueryable<Course> modifier(IQueryable<Course> q) => q.Include(c => c.Category);

            try
            {
                var (items, total) = await _uow.CourseRepo.GetAllPagedAsync(page: request.Page, itemTaken: request.ItemTaken, ct: ct, modifier: modifier);

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