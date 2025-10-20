namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public static class CookieTokens
    {
        public const string AccessToken = "AuthToken";
        public const string RefreshToken = "RefreshToken";
    }

    public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
    {
        private IHttpContextAccessor HttpContextAccessor => httpContextAccessor;

        public string? GetCookie(string key)
        {
            string? value = null;

            HttpContextAccessor.HttpContext?
                .Request.Cookies.TryGetValue(key, out value);

            return value;
        }

        public (string? accessToken, string? refreshToken) GetTokens()
        {
            var access = GetCookie(CookieTokens.AccessToken);
            var refresh = GetCookie(CookieTokens.RefreshToken);

            return (access, refresh);
        }

        public void RemoveCookie(string key)
        {
            HttpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
        }
    }
}
