using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AutomotiveApp.Infrastructure.Implementation.Utils
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenService(
            JwtSettings jwtSettings,
            UserManager<User> userManager)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Generate JWT Access Token for a user
        /// </summary>
        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var claims = await BuildClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };

            var token = _tokenHandler.CreateToken(descriptor);
            return _tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generate a secure refresh token
        /// </summary>
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// Extract claims from an expired token (used in refresh flow)
        /// </summary>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var parameters = GetTokenValidationParameters(validateLifetime: false);

            try
            {
                var principal = _tokenHandler.ValidateToken(token, parameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token algorithm");
                }

                return principal;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to extract claims from expired token: {ex}");
            }
        }

        /// <summary>
        /// Validate a token (signature, issuer, audience, expiration)
        /// </summary>
        public async Task<bool> ValidateTokenAsync(string token)
        {
            var parameters = GetTokenValidationParameters(validateLifetime: true);

            try
            {
                _tokenHandler.ValidateToken(token, parameters, out _);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return await Task.FromResult(false);
            }
        }

        /// <summary>
        /// Build claims from Identity User
        /// </summary>
        private async Task<IEnumerable<Claim>> BuildClaimsAsync(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email ?? ""),
                new(ClaimTypes.Name, user.UserName ?? ""),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            return claims;
        }

        /// <summary>
        /// Helper: Get token validation parameters
        /// </summary>
        private TokenValidationParameters GetTokenValidationParameters(bool validateLifetime)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.FromMinutes(_jwtSettings.ClockSkew)
            };
        }
    }
}
