using System.Linq.Expressions;
using AutomotiveApp.Base.Entities;
using AutomotiveApp.Shared.Models;

namespace AutomotiveApp.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class, IBaseEntity
    {

        public IQueryable<T> Query();
        Task<T?> GetByIdAsync(
            Guid id,
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default);
        Task<IEnumerable<T>> GetAllAsync(
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            CancellationToken ct = default);
        Task<PaginatedResult<T>> GetAllPagedAsync(
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            int page = 1,
            int itemTaken = 6,
            bool isRandom = false,
            CancellationToken ct = default);
        Task<T?> FirstOrDefaultAsync(
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default);
        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            CancellationToken ct = default);

        Task<PaginatedResult<T>> FindPagedAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            int page = 1,
            int itemTaken = 6,
            bool isRandom = false,
            CancellationToken ct = default);
        Task AddAsync(T entity);

        Task<Guid> AddReturnIdAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> DataExistAsync(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IQueryable<T>>? modifier = null,
        CancellationToken ct = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default);
    }
}
