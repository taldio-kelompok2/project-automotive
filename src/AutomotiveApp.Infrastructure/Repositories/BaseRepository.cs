using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Base.Entities;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AutomotiveApp.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T>
        where T : class, IBaseEntity
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        private IQueryable<T> BuildQuery(
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            Expression<Func<T, bool>>? predicate = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (modifier is not null)
                query = modifier(query);

            if (predicate is not null)
                query = query.Where(predicate);

            return query;
        }

        public async Task<T?> GetByIdAsync(
            Guid id,
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            CancellationToken ct = default)
        {

            var query = BuildQuery(modifier);
            return await query.FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            CancellationToken ct = default)
        {
            var query = BuildQuery(modifier);
            return await query.ToListAsync(ct);
        }

        public async Task<PaginatedResult<T>> GetAllPagedAsync(
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            int page = 1,
            int itemTaken = 6,
            CancellationToken ct = default)
        {
            var query = BuildQuery(modifier);
            int total = await query.CountAsync(ct);

            var items = await query
                .Skip((page - 1) * itemTaken)
                .Take(itemTaken)
                .ToListAsync(ct);

            return new PaginatedResult<T>(items, total);
        }
        public async Task<T?> FirstOrDefaultAsync(
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default)
        {
            var query = BuildQuery(modifier, predicate);
            return await query.FirstOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? modifier = null,
            CancellationToken ct = default)
        {
            var query = BuildQuery(modifier, predicate);
            return await query.ToListAsync(ct);
        }

        public async Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default)
        {
            var query = _context.Set<T>().AsQueryable();
            if (predicate is not null)
                query = query.Where(predicate);

            return await query.CountAsync(ct);
        }

        public async Task<bool> DataExistAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct: ct);
            return entity is not null;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task<Guid> AddReturnIdAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            entity.MarkUpdated();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}