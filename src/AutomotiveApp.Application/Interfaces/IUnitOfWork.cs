using AutomotiveApp.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AutomotiveApp.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository CourseRepo { get; }
        ICourseCategoryRepository CourseCategoryRepo { get; }
        ICourseSessionRepository CourseSessionRepo { get; }
        ICourseBookingRepository CourseBookingRepo { get; }
        ICartRepository CartRepo { get; }
        ICartItemRepository CartItemRepo { get; }
        IUserRepository UserRepo { get; }
        IOrderRepository OrderRepo { get; }
        IOrderItemRepository OrderItemRepo { get; }
        IInvoiceRepository InvoiceRepo { get; }
        IPaymentRepository PaymentRepo { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);

    }
}