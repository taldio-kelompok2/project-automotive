using AutomotiveApp.Domain.Entities.Auth;
using System.Security.Claims;

namespace AutomotiveApp.Application.Interfaces.Utils
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        Task<bool> ValidateTokenAsync(string token);
    }
}
