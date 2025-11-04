using AutomotiveApp.BlazorUI.Models.Auth.Context;
using AutomotiveApp.BlazorUI.Services.Implementation;
using System.Net.Http.Headers;

namespace AutomotiveApp.BlazorUI.Services.Implementation;

public class AuthMessageHandler : DelegatingHandler
{
    private readonly ICookieService _cookieService;
    private readonly ILogger<AuthMessageHandler> _logger;

    public AuthMessageHandler(ICookieService cookieService, ILogger<AuthMessageHandler> logger)
    {
        _cookieService = cookieService;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("API call to {RequestUri}", request.RequestUri);

        var accessToken = GetAccessToken();
        _logger.LogDebug("Access token available: {HasToken}", !string.IsNullOrEmpty(accessToken));

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }

    private string? GetAccessToken()
    {
        var scopeServices = CircuitServicesAccessor.Current.Value;
        if (scopeServices != null)
        {
            var userContext = scopeServices.GetRequiredService<UserContextService>();
            if (!string.IsNullOrWhiteSpace(userContext.Current.AccessToken))
            {
                return userContext.Current.AccessToken;
            }
        }

        // Fallback to cookies
        var (accessToken, _) = _cookieService.GetTokens();
        return accessToken;
    }
}