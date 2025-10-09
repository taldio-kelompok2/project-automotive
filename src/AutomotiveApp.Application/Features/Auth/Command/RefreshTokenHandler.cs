using AutoMapper;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class RefreshTokenHandler(UserManager<User> userManager, ITokenService tokenService, IJwtSettings jwtSettings)
        : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(RefreshTokenCommand req, CancellationToken ct)
        {
            try
            {
                // Get principal from expired access token
                var principal = tokenService.GetPrincipalFromExpiredToken(req.AccessToken);
                if (principal == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid access token"
                    };
                }

                // Get user ID from claims
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid token claims"
                    };
                }

                // Find user
                var user = await userManager.FindByIdAsync(userId);
                if (user == null || !user.Status)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "User not found or inactive"
                    };
                }

                // Validate refresh token
                if (user.RefreshToken != req.RefreshToken)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid refresh token"
                    };
                }

                // Check refresh token expiration
                if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    // Clear expired refresh token
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = DateTime.Now;
                    await userManager.UpdateAsync(user);

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
                await userManager.UpdateAsync(user);

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
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "An error occurred during token refresh"
                };
            }
        }
    }
}
