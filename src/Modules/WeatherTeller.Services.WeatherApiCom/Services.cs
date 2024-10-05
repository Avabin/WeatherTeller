using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Services.Core;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.WeatherApiCom.Client;
using WeatherTeller.Services.WeatherApiCom.Client.Interfaces;
using WeatherTeller.Services.WeatherApiCom.Models;

namespace WeatherTeller.Services.WeatherApiCom;

public static class Services
{
    // AddWeatherApiComClient
    public static IServiceCollection AddWeatherApiCom(this IServiceCollection services, IConfigurationSection section)
    {
        services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<WeatherApiComClient>());
        services.AddWeatherApiCore();
        var options = section.Get<WeatherApiComClientOptions>();
        options ??= new WeatherApiComClientOptions();
        services.AddSingleton<IWeatherApiComClient, WeatherApiComClient>();
        services.AddSingleton<IWeatherApiComCurrent, WeatherApiComCurrent>();
        services.AddHttpClient<IWeatherApiComCurrent, WeatherApiComCurrent>(nameof(WeatherApiComCurrent), client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl);
        });
        services.AddSingleton<IWeatherApiComForecast, WeatherApiComForecast>();
        services.AddHttpClient<IWeatherApiComForecast, WeatherApiComForecast>(nameof(WeatherApiComForecast), client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl);
        });
        services.AddOptions<WeatherApiComClientOptions>().Bind(section);
        
        services.AddSingleton<IWeatherApi, WeatherApiComWeatherApi>();
        return services;
    }
}