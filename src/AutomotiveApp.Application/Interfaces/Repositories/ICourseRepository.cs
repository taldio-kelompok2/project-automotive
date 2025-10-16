using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Models;

namespace AutomotiveApp.Application.Interfaces.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetCoursesWithCategory(CancellationToken ct);
        Task<PaginatedResult<Course>> GetCoursesWithCategoryPaged(CancellationToken ct, int page = 1, int itemTaken = 6, bool isRandom = false);
        Task<Course?> GetCourseDetailById(Guid id, CancellationToken ct);
    }
}