using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Response;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MyApp.BlazorUI.Services;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

public class AuthMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IHttpClientFactory _clientFactory;

    public AuthMessageHandler(
        ILocalStorageService localStorage,
        AuthenticationStateProvider authStateProvider,
        IHttpClientFactory clientFactory)
    {
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
        _clientFactory = clientFactory;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? token = null;
        string? refreshToken = null;

        try
        {
            // Avoid JSRuntime call if prerendering (when not yet interactive)
            if (_localStorage is not null)
            {
                token = await _localStorage.GetItemAsStringAsync("authToken");
            }
        }
        catch (InvalidOperationException)
        {
            // Happens if called during prerendering before JSRuntime is ready
            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthHandler] Failed to get token: {ex.Message}");
        }

        // Attach existing access token if available
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        // Handle 401 Unauthorized
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            try
            {
                refreshToken = await _localStorage.GetItemAsStringAsync("refreshToken");
            }
            catch (InvalidOperationException)
            {
                // Same case: ignore if prerendering
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthHandler] Failed to get refresh token: {ex.Message}");
            }

            if (!string.IsNullOrEmpty(refreshToken))
            {
                var newToken = await TryRefreshTokenAsync(token, refreshToken);

                if (!string.IsNullOrEmpty(newToken))
                {
                    try
                    {
                        await _localStorage.SetItemAsStringAsync("authToken", newToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[AuthHandler] Failed to store new token: {ex.Message}");
                    }

                    // Retry the original request with the new token
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
                else
                {
                    ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
                }
            }
        }
            return response;
        }

    private async Task<string?> TryRefreshTokenAsync(string accessToken, string refreshToken)
    {
        try
        {
            var client = _clientFactory.CreateClient("API"); // Registered in Program.cs
            var payload = new RefreshTokenRequestDto { RefreshToken = refreshToken };

            var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/refresh-token")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return null;

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();
            return apiResponse?.Data?.AccessToken; // Return new access token
        }
        catch
        {
            return null;
        }
    }
}