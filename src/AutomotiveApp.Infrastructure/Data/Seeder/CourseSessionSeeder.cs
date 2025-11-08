using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Domain.Entities.Payments;
using AutomotiveApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class CourseSessionSeeder
    {
        public static async Task SeedAsync(AppDbContext db, bool reapply = false)
        {
            if (!reapply && await db.CourseSessions.AnyAsync())
                return;

            var sessions = new List<CourseSession>();
            var courses = await db.Courses.ToListAsync();
            var random = Random.Shared;

            foreach (var course in courses)
            {
                int sessionCount = random.Next(5, 11);
                for (int i = 0; i < sessionCount; i++)
                {
                    // Random day in the next 365 days
                    var daysAhead = random.Next(1, 365);
                    var sessionDate = DateTime.UtcNow.Date.AddDays(daysAhead);

                    var capacity = random.Next(8, 50);

                    sessions.Add(new CourseSession
                    {
                        Id = Guid.NewGuid(),
                        CourseId = course.Id,
                        Date = sessionDate,
                        Capacity = (uint)capacity
                    });
                }

            }

            await db.CourseSessions.AddRangeAsync(sessions);
            await db.SaveChangesAsync();
        }
    }
}