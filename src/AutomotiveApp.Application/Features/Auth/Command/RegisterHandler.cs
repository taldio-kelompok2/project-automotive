using AutoMapper;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class RegisterHandler(UserManager<User> userManager, IMapper mapper, ITokenService tokenService, IJwtSettings jwtSettings)
        : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(RegisterCommand req, CancellationToken ct)
        {
            var existingUser = await userManager.FindByEmailAsync(req.RegisterRequestDto.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "User with this email already exists"
                };
            }

            var user = mapper.Map<User>(req.RegisterRequestDto);

            var result = await userManager.CreateAsync(user, req.RegisterRequestDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Registration failed: {errors}"
                };
            }

            // Assign default role "Buyer"
            await userManager.AddToRoleAsync(user, "Buyer");

            // Add default claims
            await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("can_view_profile", "true"));

            // Generate JWT tokens
            var accessToken = await tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = tokenService.GenerateRefreshToken();

            // Save refresh token ke database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays);
            await userManager.UpdateAsync(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Registration successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
            };
        }
    }
}
