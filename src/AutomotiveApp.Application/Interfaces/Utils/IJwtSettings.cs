namespace AutomotiveApp.Application.Interfaces.Utils
{
    public interface IJwtSettings
    {
        string SecretKey { get; }
        string Issuer { get; }
        string Audience { get; }
        int AccessTokenExpirationMinutes { get; }
        int RefreshTokenExpirationDays { get; }
        bool ValidateIssuer { get; }
        bool ValidateAudience { get; }
        bool ValidateLifetime { get; }
        bool ValidateIssuerSigningKey { get; }
        int ClockSkew { get; }
    }
}
