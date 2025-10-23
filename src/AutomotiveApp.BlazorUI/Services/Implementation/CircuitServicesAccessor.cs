using Microsoft.AspNetCore.Components.Server.Circuits;

namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class CircuitServicesAccessor
    {
        public static readonly AsyncLocal<IServiceProvider?> Current = new();
    }

    public class BlazorScopeCircuitHandler : CircuitHandler
    {
        private readonly IServiceProvider _services;

        public BlazorScopeCircuitHandler(IServiceProvider services)
        {
            _services = services;
        }

        public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(Func<CircuitInboundActivityContext, Task> next)
            => async ctx =>
            {
                CircuitServicesAccessor.Current.Value = _services;
                await next(ctx);
                CircuitServicesAccessor.Current.Value = null;
            };
    }
}
