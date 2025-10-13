using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.CourseSessions.Queries
{
    public class GetCourseSessionByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCourseSessionById, CourseSessionQueryDto>
    {
        public async Task<CourseSessionQueryDto> Handle(GetCourseSessionById request, CancellationToken ct)
        {
            static IQueryable<CourseSession> modifier(IQueryable<CourseSession> q) => q.Include(c => c.Course);

            try
            {
                var item = await uow.CourseSessionRepo.GetByIdAsync(request.Id, modifier, ct: ct);
                return mapper.Map<CourseSessionQueryDto>(item);
            }
            catch { throw; }
        }
    }
}