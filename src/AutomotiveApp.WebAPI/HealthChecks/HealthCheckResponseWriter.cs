using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AutomotiveApp.WebAPI.HealthChecks;

public static class HealthCheckResponseWriter
{
    public static Task WriteResponse(HttpContext httpContext, HealthReport result)
    {
        httpContext.Response.ContentType = "application/json";

        var payload = new
        {
            status = result.Status.ToString(),
            entries = result.Entries.Select(e => new
            {
                key = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                data = e.Value.Data
            }),
            totalDuration = result.TotalDuration
        };

        return httpContext.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
