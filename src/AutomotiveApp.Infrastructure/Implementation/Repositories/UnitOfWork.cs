using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class UnitOfWork(
    AppDbContext context,
    ICourseRepository courseRepo,
    ICourseCategoryRepository courseCategoryRepo,
    ICourseSessionRepository courseSessionRepo,
    ICourseBookingRepository courseBookingRepo,
    ICartRepository cartRepo,
    ICartItemRepository cartItemRepo,
    IUserRepository userRepo,
    IOrderRepository orderRepo,
    IOrderItemRepository orderItemRepo,
    IInvoiceRepository InvoiceRepo,
    IPaymentRepository PaymentRepo
    ) : IUnitOfWork
    {
        ICourseRepository IUnitOfWork.CourseRepo => courseRepo;
        ICourseCategoryRepository IUnitOfWork.CourseCategoryRepo => courseCategoryRepo;
        ICourseSessionRepository IUnitOfWork.CourseSessionRepo => courseSessionRepo;
        ICourseBookingRepository IUnitOfWork.CourseBookingRepo => courseBookingRepo;
        IUserRepository IUnitOfWork.UserRepo => userRepo;
        ICartRepository IUnitOfWork.CartRepo => cartRepo;
        ICartItemRepository IUnitOfWork.CartItemRepo => cartItemRepo;
        IOrderRepository IUnitOfWork.OrderRepo => orderRepo;
        IOrderItemRepository IUnitOfWork.OrderItemRepo => orderItemRepo;
        IInvoiceRepository IUnitOfWork.InvoiceRepo => InvoiceRepo;
        IPaymentRepository IUnitOfWork.PaymentRepo => PaymentRepo;

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

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            return await Context.Database.BeginTransactionAsync(ct);
        }
    }
}