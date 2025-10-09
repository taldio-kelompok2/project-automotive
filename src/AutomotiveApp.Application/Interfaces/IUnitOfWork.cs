using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository CourseRepo { get; }
        ICourseCategoryRepository CourseCategoryRepo { get; }
        ICourseSessionRepository CourseSessionRepo { get; }
        ICourseBookingRepository CourseBookingRepo { get; }
        IUserRepository UserRepo { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}