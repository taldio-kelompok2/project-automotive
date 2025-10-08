using AutomotiveApp.Shared.Dtos.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public record GetUserById(Guid UserId) : IRequest<UserQueryDto>;
}
