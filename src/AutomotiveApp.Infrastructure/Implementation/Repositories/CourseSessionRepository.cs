using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Infrastructure.Repositories;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class CourseSessionRepository(AppDbContext context) : BaseRepository<CourseSession>(context), ICourseSessionRepository { }
}