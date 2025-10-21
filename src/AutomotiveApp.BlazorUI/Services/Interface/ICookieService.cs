namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public interface ICookieService
    {
        public string? GetCookie(string key);

        public (string? accessToken, string? refreshToken) GetTokens();

        public void RemoveCookie(string key);
    }
}