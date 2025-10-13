using System.Linq.Expressions;
using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Implementation.Repositories
{
    public class UserRepository(UserManager<User> userManager) : IUserRepository
    {

        public IQueryable<User> Query()
        {
            return userManager.Users;
        }

        public async Task AddAsync(User entity)
        {
            var result = await userManager.CreateAsync(entity);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<Guid> AddReturnIdAsync(User entity)
        {
            await AddAsync(entity);
            return entity.Id;
        }

        public async Task<int> CountAsync(Expression<Func<User, bool>>? predicate = null, CancellationToken ct = default)
        {
            if (predicate != null)
                return await userManager.Users.CountAsync(predicate, ct);

            return await userManager.Users.CountAsync(ct);
        }

        public async Task<bool> DataExistAsync(
            Expression<Func<User, bool>> predicate,
            Func<IQueryable<User>, IQueryable<User>>? modifier = null,
            CancellationToken ct = default)
        {
            IQueryable<User> query = userManager.Users;

            if (modifier != null)
                query = modifier(query);

            return await query.AnyAsync(predicate, ct);
        }

        public async Task<IEnumerable<User>> FindAsync(
            Expression<Func<User, bool>> predicate,
            Func<IQueryable<User>, IQueryable<User>>? modifier = null,
            CancellationToken ct = default)
        {
            IQueryable<User> query = userManager.Users;

            if (modifier != null)
                query = modifier(query);

            return await query.Where(predicate).ToListAsync(ct);
        }

        public async Task<User?> FirstOrDefaultAsync(
            Func<IQueryable<User>, IQueryable<User>>? modifier = null,
            Expression<Func<User, bool>>? predicate = null,
            CancellationToken ct = default)
        {
            IQueryable<User> query = userManager.Users;

            if (modifier != null)
                query = modifier(query);

            if (predicate != null)
                return await query.FirstOrDefaultAsync(predicate, ct);

            return await query.FirstOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<User>> GetAllAsync(
            Func<IQueryable<User>, IQueryable<User>>? modifier = null,
            CancellationToken ct = default)
        {
            IQueryable<User> query = userManager.Users;

            if (modifier != null)
                query = modifier(query);

            return await query.ToListAsync(ct);
        }

        public async Task<PaginatedResult<User>> GetAllPagedAsync(
            Func<IQueryable<User>, IQueryable<User>>? modifier = null,
            int page = 1,
            int itemTaken = 6,
            CancellationToken ct = default)
        {
            IQueryable<User> query = userManager.Users;

            if (modifier != null)
                query = modifier(query);

            var totalItems = await query.CountAsync(ct);
            var items = await query
                .Skip((page - 1) * itemTaken)
                .Take(itemTaken)
                .ToListAsync(ct);

            return new PaginatedResult<User>(items, totalItems);
        }

        public async Task<User?> GetByIdAsync(Guid id, Func<IQueryable<User>, IQueryable<User>>? modifier = null, CancellationToken ct = default)
        {
            IQueryable<User> query = userManager.Users;

            if (modifier != null)
                query = modifier(query);

            return await query.FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public void Update(User entity)
        {
            var result = userManager.UpdateAsync(entity).GetAwaiter().GetResult();
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public void Delete(User entity)
        {
            var result = userManager.DeleteAsync(entity).GetAwaiter().GetResult();
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}