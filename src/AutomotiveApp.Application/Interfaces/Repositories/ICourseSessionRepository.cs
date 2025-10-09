using AutomotiveApp.Domain.Entities.Courses;

namespace AutomotiveApp.Application.Interfaces.Repositories
{
    public interface ICourseSessionRepository : IRepository<CourseSession>
    {
        Task<bool> IsSessionAvailable(Guid id, CancellationToken ct);
    }
}