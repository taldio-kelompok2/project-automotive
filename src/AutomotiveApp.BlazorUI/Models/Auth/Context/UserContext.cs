using System.Security.Claims;
using MudBlazor;

namespace AutomotiveApp.BlazorUI.Models.Auth.Context
{
    public record UserContext
    {
        public bool IsAuthenticated { get; init; }
        public Guid Id { get; init; }
        public string Role { get; init; } = default!;

        public UserContext Update(ClaimsPrincipal user)
        {
            var nameIdClaim = user.FindFirst(c => c.Type == "nameid")?.Value;
            return this with
            {
                IsAuthenticated = user.Identity?.IsAuthenticated ?? false,
                Role = user.FindFirst(c => c.Type == "role")?.Value ?? "Guest"
            };
        }
    }
}