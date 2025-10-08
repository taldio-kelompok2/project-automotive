namespace AutomotiveApp.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository CourseRepo { get; }
        ICourseCategoryRepository CourseCategoryRepo { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}