using System.Linq.Expressions;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Models;

namespace AutomotiveApp.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
    }
}