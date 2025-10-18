using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Models;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface IUserService
    {
        Task<PaginatedResult<UserQueryDto>> GetPagedUsers(int page, int itemTaken, string? search);
        Task<bool> CreateUser(UserCreateRequestDto userCreateDto);
        Task<bool> UpdateUser(Guid userId, UserUpdateRequestDto userUpdateDto);

        Task<IEnumerable<UserQueryDto>> GetAllUsers();

    }
}