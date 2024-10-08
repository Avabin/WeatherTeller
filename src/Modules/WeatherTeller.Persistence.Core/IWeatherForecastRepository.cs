using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.Persistence.Core;

public interface IWeatherForecastRepository
{
    Task<ulong> AddWeatherForecastAsync(WeatherForecast weatherForecast);
    IAsyncEnumerable<WeatherForecast> GetWeatherForecastsAsync();
    Task RemoveWeatherForecastAsync(ulong id);
}