using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Interface;
using AutomotiveApp.Infrastructure.Base;
using AutomotiveApp.Infrastructure.Data;

namespace AutomotiveApp.Infrastructure.Repositories
{
    public class GenericRepository<T> : BaseRepository<T>, IRepository<T>
        where T : class, IBaseEntity
    {
        public GenericRepository(AppDbContext context) : base(context) { }
    }
}
