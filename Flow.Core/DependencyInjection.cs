using Microsoft.Extensions.DependencyInjection;
using Flow.Core.Services;

namespace Flow.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IGreetingService, GreetingService>();
        return services;
    }
} 