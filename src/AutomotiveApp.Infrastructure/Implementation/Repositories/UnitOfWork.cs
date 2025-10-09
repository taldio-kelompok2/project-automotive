using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Infrastructure.Data;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class UnitOfWork(
    AppDbContext context,
    ICourseRepository courseRepo,
    ICourseCategoryRepository courseCategoryRepo,
    ICourseSessionRepository courseSessionRepo,
    ICourseBookingRepository courseBookingRepo,
    IUserRepository userRepo
    ) : IUnitOfWork
    {
        ICourseRepository IUnitOfWork.CourseRepo => courseRepo;
        ICourseCategoryRepository IUnitOfWork.CourseCategoryRepo => courseCategoryRepo;
        ICourseSessionRepository IUnitOfWork.CourseSessionRepo => courseSessionRepo;
        ICourseBookingRepository IUnitOfWork.CourseBookingRepo => courseBookingRepo;
        IUserRepository IUnitOfWork.UserRepo => userRepo;
        private AppDbContext Context => context;

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this); //Dont call the Garbage Collector because its already disposed manually.
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await Context.SaveChangesAsync(ct);
        }
    }
}