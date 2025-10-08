using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Domain.Entities.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public class GetCourseByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCourseById, CourseQueryDto>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        public async Task<CourseQueryDto> Handle(GetCourseById request, CancellationToken ct)
        {
            static IQueryable<Course> modifier(IQueryable<Course> q) => q.Include(c => c.Category);

            try
            {
                var item = await _uow.CourseRepo.GetByIdAsync(request.Id, modifier, ct);
                return _mapper.Map<CourseQueryDto>(item);
            }
            catch
            {
                throw;
            }
        }
    }
}