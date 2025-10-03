using System.Linq.Expressions;
using AutomotiveApp.Base.Entities;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Domain.Interface
{
    public interface IRepository<T> where T : class, IBaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> DataExistAsync(Guid id);
        Task<int> CountAsync();

        // Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
