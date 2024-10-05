using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Services.Core.WeatherApi;

namespace WeatherTeller.Services.Core;

public static class Services
{
    public static IServiceCollection AddWeatherApiCore(this IServiceCollection services)
    {
        services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<IWeatherApi>());
        return services;
    }
}