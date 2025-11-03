using AutoMapper;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class RegisterHandler(UserManager<User> userManager, IMapper mapper, ITokenService tokenService, IJwtSettings jwtSettings, IEmailService emailService)
        : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(RegisterCommand req, CancellationToken cancellationToken)
        {
            var existingUsername = await userManager.FindByNameAsync(req.RegisterRequestDto.UserName);
            if (existingUsername != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Username is invalid"
                };
            }

            var existingEmail = await userManager.FindByEmailAsync(req.RegisterRequestDto.Email);
            if (existingEmail != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email is invalid"
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

            await userManager.AddToRoleAsync(user, "Buyer");
            await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("can_view_profile", "true"));

            var accessToken = await tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays);
            await userManager.UpdateAsync(user);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await emailService.SendConfirmationEmailAsync(user.Email!, user.Id.ToString(), token);

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
