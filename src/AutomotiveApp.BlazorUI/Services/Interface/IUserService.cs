using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface IUserService
    {
        Task<PaginatedResult<UserQueryDto>> GetPagedUsers(int page, int itemTaken, string? search);
        Task<ApiResponse<UserCreateRequestDto>> CreateUser(UserCreateRequestDto userCreateDto);
        Task<ApiResponse<UserUpdateRequestDto>> UpdateUser(Guid userId, UserUpdateRequestDto userUpdateDto);
        Task<IEnumerable<UserQueryDto>> GetAllUsers();

    }
}