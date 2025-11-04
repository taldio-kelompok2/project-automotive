using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace AutomotiveApp.WebAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get or generate a correlation ID
            var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
                                ?? GenerateCorrelationId();

            // Attach correlation ID to response headers
            context.Response.Headers[CorrelationIdHeader] = correlationId;

            var requestPath = context.Request.Path;
            var method = context.Request.Method;
            //var start = Stopwatch.StartNew();

            // Enrich log context with correlation info
            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("RequestPath", requestPath))
            using (LogContext.PushProperty("HttpMethod", method))
            {
                //Log.Information("➡️ Incoming {Method} {Path}", method, requestPath);

                try
                {
                    await _next(context);
                    //start.Stop();

                    //Log.Information("✅ Completed {Method} {Path} with {StatusCode} in {Elapsed:0.000}s",
                        //method, requestPath, context.Response.StatusCode, start.Elapsed.TotalSeconds, correlationId);
                }
                catch (Exception ex)
                {
                    //start.Stop();
                    Log.Error(ex, "❌ Error processing {Method} {Path}", method, requestPath, correlationId);
                    throw;
                }
            }
        }

        private static string GenerateCorrelationId()
        {
            return Guid.NewGuid().ToString("N")[..10];
        }
    }

    public class ShortSourceContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Properties.TryGetValue("SourceContext", out var value))
            {
                var fullName = value.ToString().Trim('"');
                var shortName = fullName.Split('.').LastOrDefault() ?? fullName;
                var shortProp = propertyFactory.CreateProperty("ShortSourceContext", "[" + shortName + "]");
                logEvent.AddPropertyIfAbsent(shortProp);
            }
        }
    }
}
