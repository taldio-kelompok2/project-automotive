using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AutomotiveApp.WebAPI.HealthChecks;

public class MemoryHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken token = default)
    {
        var totalMem = GC.GetTotalMemory(false);

        var data = new Dictionary<string, object>
        {
            ["TotalMemoryBytes"] = totalMem
        };

        return Task.FromResult(HealthCheckResult.Healthy("OK", data));
    }
}
