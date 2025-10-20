using AutomotiveApp.BlazorUI.Services.Implementation;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Headers;

public class AuthMessageHandler : DelegatingHandler
{
    private readonly ICookieService _cookieService;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthMessageHandler(
        AuthenticationStateProvider authenticationStateProvider,
        IHttpClientFactory clientFactory,
        ICookieService cookieService
    )
    {

        _authStateProvider = authenticationStateProvider;
        _cookieService = cookieService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var (accessToken, _) = _cookieService.GetTokens();

        if (!string.IsNullOrWhiteSpace(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        // If server rejects -> logout
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }

        return response;
    }
}
