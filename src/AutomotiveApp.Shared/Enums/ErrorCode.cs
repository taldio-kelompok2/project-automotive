namespace AutomotiveApp.Shared.Enums
{
    public enum HttpCode
    {
        OK = 200,
        BadRequest = 400,       // Invalid request / validation failed
        Unauthorized = 401,     // Not authenticated
        Forbidden = 403,        // Not authorized
        NotFound = 404,         // Resource not found
        Conflict = 409,         // Conflict (e.g., duplicate)
        InternalServerError = 500  // Server error
    }
}