using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Services.Settings;
using WeatherTeller.Services.WeatherApi;
using WeatherTeller.Services.WeatherApi.Publishers;

namespace WeatherTeller.Services;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHostedService<CheckGeolocationHostedService>();
        services.AddHostedService<LoadSettingsStartupTask>();
        services.AddHostedService<DaysForecastPublisher>();
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining<DaysForecastPublisher>();
        });

        return services;
    }
}