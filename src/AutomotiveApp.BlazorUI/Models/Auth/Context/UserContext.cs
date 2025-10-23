using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AutomotiveApp.BlazorUI.Models.Auth.Context
{
    public record UserContext
    {
        public bool IsAuthenticated { get; init; }
        public string? AccessToken { get; init; }
        public Guid Id { get; init; }
        public string Role { get; init; } = default!;
        public DateTime? ExpiresAtUtc { get; init; }

        public static UserContext Guest => new()
        {
            IsAuthenticated = false,
            Id = Guid.Empty,
            Role = "Guest",
            AccessToken = null,
            ExpiresAtUtc = null
        };

        public static ClaimsPrincipal ParsePrincipalFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var identity = new ClaimsIdentity(token.Claims, "jwt");
            return new ClaimsPrincipal(identity);
        }

        private static DateTime? ExtractExpiryFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(jwt))
                return null;

            var token = handler.ReadJwtToken(jwt);
            var expClaim = token.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (long.TryParse(expClaim, out var unixSeconds))
                return DateTimeOffset.FromUnixTimeSeconds(unixSeconds).UtcDateTime;

            return null;
        }

        public UserContext Update(string accessToken)
        {
            try
            {
                var principal = ParsePrincipalFromJwt(accessToken);

                var nameIdClaim = principal.FindFirst(c => c.Type == "nameid")?.Value;
                if (string.IsNullOrWhiteSpace(nameIdClaim) || !Guid.TryParse(nameIdClaim, out var parsedId))
                {
                    return Guest;
                }

                var expiresAt = ExtractExpiryFromJwt(accessToken);

                return this with
                {
                    AccessToken = accessToken,
                    Id = parsedId,
                    IsAuthenticated = principal.Identity?.IsAuthenticated ?? false,
                    Role = principal.FindFirst(c => c.Type == "role")?.Value ?? "Guest",
                    ExpiresAtUtc = expiresAt
                };
            }
            catch
            {
                return Guest;
            }
        }

    }
}
