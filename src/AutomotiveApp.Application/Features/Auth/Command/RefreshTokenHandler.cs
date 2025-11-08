using AutoMapper;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class RefreshTokenHandler(UserManager<User> userManager, ITokenService tokenService, IJwtSettings jwtSettings, ILogger<RefreshTokenHandler> logger)
    : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(RefreshTokenCommand req, CancellationToken cancellationToken)
        {
            try
            {
                var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == req.RefreshToken, cancellationToken);

                if (user == null || !user.Status)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "user with this token is not found"
                    };
                }

                logger.LogInformation("POST /api/auth/refresh-token - Refresh token request from Email={Email}, RefreshToken={RefreshToken}",
                    user.Email,
                    req.RefreshToken);

                if (DateTime.UtcNow > user.RefreshTokenExpiryTime)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow;
                    await userManager.UpdateAsync(user);
                    logger.LogWarning("Refresh token failed Error=Refresh token expired");
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Refresh token expired. Please login again."
                    };
                }

                // Generate new tokens
                var newAccessToken = await tokenService.GenerateAccessTokenAsync(user);
                var newRefreshToken = tokenService.GenerateRefreshToken();

                // Update user with new refresh token
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays);
                user.LastLogin = DateTime.UtcNow;
                await userManager.UpdateAsync(user);

                logger.LogInformation("POST /api/auth/refresh-token - Token refreshed with RefreshToken={RefreshToken}, AccessToken={AccessToken}",
                    newRefreshToken,
                    newAccessToken);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    AccessTokenExpiry = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
                };
            }
            catch (Exception)
            {
                logger.LogWarning("Refresh token failed Error=Internal error");
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "An error occurred during token refresh"
                };
            }
        }
    }
}