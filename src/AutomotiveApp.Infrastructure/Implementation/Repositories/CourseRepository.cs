using System.Diagnostics.Eventing.Reader;
using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Infrastructure.Repositories;
using AutomotiveApp.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class CourseRepository(AppDbContext context) : BaseRepository<Course>(context), ICourseRepository
    {

        private static Func<IQueryable<Course>, IQueryable<Course>> CourseAvailableModifier =>
            query => query
                .Include(c => c.Category)
                .Include(
                    c => c.Sessions
                    .Where(s => s.Capacity > s.Bookings.Count)
                );
        public async Task<IEnumerable<Course>> GetCoursesWithCategory(CancellationToken ct)
        {
            return await GetAllAsync(
                modifier: q => q
                    .Include(c => c.Category),
                ct: ct
                );
        }

        public async Task<PaginatedResult<Course>> GetCoursesWithCategoryPaged(CancellationToken ct, int page = 1, int itemTaken = 6)
        {
            return await GetAllPagedAsync(
                page: page,
                itemTaken: itemTaken,
                modifier: q => q
                        .Include(c => c.Category),
                    ct: ct
                    );
        }

        public async Task<Course?> GetCourseDetailById(Guid id, CancellationToken ct)
        {
            return await GetByIdAsync(
                id: id,
                modifier: CourseAvailableModifier,
                ct: ct
            );
        }
    }
}