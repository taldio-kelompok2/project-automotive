using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class LoginHandler(UserManager<User> userManager, ITokenService tokenService, IJwtSettings jwtSettings, SignInManager<User> signInManager, ILogger<LoginHandler> logger)
        : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        public async Task<LoginResponseDto> Handle(LoginCommand req, CancellationToken cancellationToken)
        {
            // Cari user berdasarkan email
            var user = await userManager.FindByEmailAsync(req.LoginRequestDto.Email);
            if (user == null || !user.Status)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Validasi password dengan lockout protection
            var result = await signInManager.CheckPasswordSignInAsync(user, req.LoginRequestDto.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    SignInResult = result
                };
            }

            // Generate JWT tokens
            var accessToken = await tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = tokenService.GenerateRefreshToken();

            // Save refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays);
            user.LastLogin = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            return new LoginResponseDto
            {
                Success = true,
                Message = "Login successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
            };
        }
    }
}
