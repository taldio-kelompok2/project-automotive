using AutomotiveApp.BlazorUI.Models.Auth.Context;
using AutomotiveApp.BlazorUI.Services.Implementation;
using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Response;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace AutomotiveApp.BlazorUI.Services.Implementation;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ICookieService _cookieService;
    private readonly UserContextService _userContextService;
    private readonly IJSRuntime _js;
    private readonly ILogger<CustomAuthStateProvider> _logger;

    private readonly AuthenticationState _anonymous;

    public CustomAuthStateProvider(
        ICookieService cookieService,
        UserContextService userContextService,
        IJSRuntime js,
        ILogger<CustomAuthStateProvider> logger)
    {
        _cookieService = cookieService;
        _userContextService = userContextService;
        _js = js;
        _logger = logger;

        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var accessToken = _userContextService.Current.AccessToken;

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            (accessToken, _) = _cookieService.GetTokens();
        }

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            _logger.LogInformation("No access token found. Setting user as anonymous.");
            return _anonymous;
        }

        try
        {
            _userContextService.Update(accessToken);
            var principal = UserContext.ParsePrincipalFromJwt(accessToken);

            _logger.LogInformation("User authenticated. NameId: {NameId}, Role: {Role}",
                principal.FindFirst("nameid")?.Value,
                principal.FindFirst("role")?.Value);

            return new AuthenticationState(principal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse or update authentication state. Clearing context.");
            _userContextService.Clear();
            return _anonymous;
        }
    }

    public async Task<bool> TryRefreshSessionAsync()
    {
        var (_, refreshToken) = _cookieService.GetTokens();

        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            try
            {
                var result = await _js.InvokeAsync<string>("refreshViaFetch", "/auth/proxy-refresh-token");
                var response = JsonSerializer.Deserialize<ApiResponse<AuthResponseDto>>(result);
                if (response?.Data?.Success ?? false)
                {
                    _logger.LogInformation(
                        "Refresh successful at {RefreshTime}. Updating authentication state with token (Expires: {ExpiryTime}).",
                        DateTime.Now,
                        _userContextService.Current.ExpiresAtUtc?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Unknown");
                    NotifyUserAuthentication(response.Data.AccessToken);
                    return true;
                }
                else
                {
                    _logger.LogInformation("Refresh failed. message: {message}", response?.Data?.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while attempting to refresh token.");
                return false;
            }
        }
        else
        {
            _logger.LogInformation("Refresh skipped. Access token still valid or no refresh token available.");
            return true;
        }
    }

    public void NotifyUserAuthentication(string token)
    {
        try
        {
            var principal = UserContext.ParsePrincipalFromJwt(token);
            _userContextService.Update(token);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

            _logger.LogInformation("Authentication updated. User ID: {UserId}, Role: {Role}",
                principal.FindFirst("nameid")?.Value,
                principal.FindFirst("role")?.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to notify authentication state.");
        }
    }

    public void NotifyUserLogout()
    {
        _logger.LogInformation("Notifying user logout...");
        _userContextService.Clear();
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        _logger.LogInformation("User logged out and authentication state cleared.");
    }
}
