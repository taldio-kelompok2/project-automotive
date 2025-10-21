using AutomotiveApp.BlazorUI.Services.Implementation;
using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Response;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ICookieService _cookieService;
    private readonly IJSRuntime _js;
    private readonly AuthenticationState _anonymous;

    public CustomAuthStateProvider(ICookieService cookieService, IJSRuntime js)
    {
        _cookieService = cookieService;
        _js = js;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var (accessToken, _) = _cookieService.GetTokens();
        if (string.IsNullOrWhiteSpace(accessToken))
            return _anonymous;

        var claims = ParseClaimsFromJwt(accessToken);
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        return new AuthenticationState(user);
    }

    public async Task TryRefreshSessionAsync()
    {
        var (accessToken, refreshToken) = _cookieService.GetTokens();

        if (string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrWhiteSpace(refreshToken))
        {
            try
            {
                var result = await _js.InvokeAsync<string>("refreshViaFetch", "/auth/proxy-refresh-token");
                var response = JsonSerializer.Deserialize<ApiResponse<AuthResponseDto>>(result);

                if (response?.Data?.AccessToken is not null)
                {
                    NotifyUserAuthentication(response.Data.AccessToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthState] Manual refresh failed: {ex.Message}");
            }
        }
    }

    public void NotifyUserAuthentication(string token)
    {
        var claims = ParseClaimsFromJwt(token);
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        return token.Claims;
    }
}
