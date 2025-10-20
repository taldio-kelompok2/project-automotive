using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class LoginHandler(UserManager<User> userManager, ITokenService tokenService, IJwtSettings jwtSettings, SignInManager<User> signInManager)
        : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(LoginCommand req, CancellationToken ct)
        {
            // Cari user berdasarkan email
            var user = await userManager.FindByEmailAsync(req.LoginRequestDto.Email);
            if (user == null || !user.Status)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Validasi password dengan lockout protection
            var result = await signInManager.CheckPasswordSignInAsync(user, req.LoginRequestDto.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                //_logger.LogWarning("Login failed for email: {Email}. Reason: {Reason}", request.Email, result.ToString());

                if (result.IsLockedOut)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Account is locked out. Please try again later."
                    };
                }

                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Generate JWT tokens
            var accessToken = await tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = tokenService.GenerateRefreshToken();

            // Save refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays);
            await userManager.UpdateAsync(user);

            //_logger.LogInformation("Login successful for email: {Email}", request.Email);

            Console.WriteLine($"Token Expires: {DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes)}");

            return new AuthResponseDto
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
