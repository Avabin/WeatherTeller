using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Persistence.Core;
using WeatherTeller.Persistence.Settings;
using WeatherTeller.Persistence.WeatherForecasts;

namespace WeatherTeller.Persistence;

public static class Services
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<ISettingsRepository, SettingsRepository>();
        services.AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }
}