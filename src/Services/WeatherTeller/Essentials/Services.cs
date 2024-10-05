using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Essentials.Handlers;

namespace WeatherTeller.Essentials;

internal static class Services
{
    public static IServiceCollection AddEssentials(this IServiceCollection services)
    {
        services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<GetGeolocationHandler>());

        return services;
    }
}