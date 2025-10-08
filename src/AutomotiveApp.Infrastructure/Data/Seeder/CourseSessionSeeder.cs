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
            {
                return;
            }

            var sessions = new List<CourseSession>();
            var courses = await db.Courses.ToListAsync();

            foreach (var course in courses)
            {
                sessions.Add(new CourseSession
                {
                    Id = Guid.NewGuid(),
                    CourseId = course.Id,
                    Date = DateTime.UtcNow.Date.AddDays(1), // besok
                    Capacity = 10
                });

                sessions.Add(new CourseSession
                {
                    Id = Guid.NewGuid(),
                    CourseId = course.Id,
                    Date = DateTime.UtcNow.Date.AddDays(8), // minggu depan
                    Capacity = 12
                });

                sessions.Add(new CourseSession
                {
                    Id = Guid.NewGuid(),
                    CourseId = course.Id,
                    Date = DateTime.UtcNow.Date.AddDays(15), // dua minggu lagi
                    Capacity = 15
                });
            }

            await db.CourseSessions.AddRangeAsync(sessions);
            await db.SaveChangesAsync();
        }
    }
}