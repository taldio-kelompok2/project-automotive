using System.Linq.Expressions;
using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.CourseSessions.Queries
{
    public record GetCourseSessionHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCourseSessions, IEnumerable<CourseSessionQueryDto>>
    {
        public async Task<IEnumerable<CourseSessionQueryDto>> Handle(GetCourseSessions request, CancellationToken ct)
        {
            static IQueryable<CourseSession> modifier(IQueryable<CourseSession> q)
            => q.Include(c => c.Course);

            Expression<Func<CourseSession, bool>> predicate;
            try
            {
                if (request.CourseId != null)
                    predicate = c => c.CourseId == request.CourseId;
                else
                    predicate = c => true;

                var items = await uow.CourseSessionRepo.FindAsync(
                    predicate,
                    modifier,
                    ct
                );

                var mappedItems = mapper.Map<IEnumerable<CourseSessionQueryDto>>(items);
                return mappedItems;
            }
            catch
            {
                throw;
            }
        }
    }
}