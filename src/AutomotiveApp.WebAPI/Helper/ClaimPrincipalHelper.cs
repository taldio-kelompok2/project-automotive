using System.Security.Claims;

namespace AutomotiveApp.WebAPI.Helper
{
    public static class ClaimPrincipalHelper
    {
        public static Guid? GetCurrentUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var guid) ? guid : null;
        }
    }
}