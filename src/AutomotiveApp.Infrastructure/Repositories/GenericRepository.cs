using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Base.Entities;
using AutomotiveApp.Infrastructure.Data;

namespace AutomotiveApp.Infrastructure.Repositories
{
    public class GenericRepository<T> : BaseRepository<T>, IRepository<T>
        where T : class, IBaseEntity
    {
        public GenericRepository(AppDbContext context) : base(context) { }
    }
}
