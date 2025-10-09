using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Infrastructure.Repositories;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class CourseSessionRepository(AppDbContext context)
    : BaseRepository<CourseSession>(context), ICourseSessionRepository
    {
        public async Task<bool> IsSessionAvailable(Guid id, CancellationToken ct)
        {
            return await DataExistAsync(
                predicate: s => s.Id == id && s.Capacity > s.Bookings.Count,
                ct: ct
            );
        }
    }
}