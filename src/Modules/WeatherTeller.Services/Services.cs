using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.Settings;
using WeatherTeller.Services.WeatherApi;
using WeatherTeller.Services.WeatherApi.Publishers;

namespace WeatherTeller.Services;

public static class Services
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ISettingsRepository, SettingsRepository>();
        
        services.AddHostedService<RefreshingBackgroundService>();
        services.AddHostedService<LoadSettingsStartupTask>();
        services.AddHostedService<CurrentWeatherStatePublisher>();
        services.AddHostedService<TomorrowForecastPublisher>();
        services.AddHostedService<DaysForecastPublisher>();
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining<SettingsRepository>();
        });
    }
}