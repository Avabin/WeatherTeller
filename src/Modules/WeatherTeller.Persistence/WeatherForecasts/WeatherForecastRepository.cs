using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using Riok.Mapperly.Abstractions;
using WeatherTeller.Persistence.Core;
using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.Persistence.WeatherForecasts;

internal class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly ILogger<WeatherForecastRepository> _logger;
    private readonly IWeatherDataSource _weatherDataSource;

    public WeatherForecastRepository(IWeatherDataSource weatherDataSource, ILogger<WeatherForecastRepository> logger)
    {
        _weatherDataSource = weatherDataSource;
        _logger = logger;
    }

    public async Task<ulong> AddWeatherForecastAsync(WeatherForecast weatherForecast)
    {
        _logger.LogInformation("Adding weather forecast for location {@Location}", weatherForecast.Location);

        var snapshot = weatherForecast.ToSnapshot();
        var id = await _weatherDataSource.Add(snapshot);

        _logger.LogInformation("Added weather forecast for location {@Location} with id {Id}", weatherForecast.Location,
            id);

        return id;
    }

    public IAsyncEnumerable<WeatherForecast> GetWeatherForecastsAsync()
    {
        _logger.LogInformation("Getting all weather forecasts");

        return _weatherDataSource
            .Where()
            .Select(x => x.ToModel());
    }

    public async Task<WeatherForecast?> GetLatestWeatherForecastAsync()
    {
        _logger.LogInformation("Getting latest weather forecast");

        var snapshot = await _weatherDataSource
            .Where()
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        var model = snapshot?.ToModel();

        if (model is not null)
            _logger.LogDebug("Got latest weather forecast for location {@Location}", model.Location);
        else
            _logger.LogInformation("No weather forecast found");

        return model;
    }

    public async Task RemoveWeatherForecastAsync(ulong id)
    {
        _logger.LogInformation("Removing weather forecast with id {Id}", id);

        await _weatherDataSource.RemoveOne(id);

        _logger.LogInformation("Removed weather forecast with id {Id}", id);
    }

    public async Task RemoveWeatherForecastBeforeAsync(DateTimeOffset date)
    {
        _logger.LogInformation("Removing weather forecasts before {Date}", date);

        await _weatherDataSource.RemoveMany(x => x.CreatedAt < date);

        _logger.LogInformation("Removed weather forecasts before {Date}", date);
    }
}

[Mapper]
public static partial class WeatherSnapshotExtensions
{
    // From WeatherForecast to WeatherForecastSnapshot
    public static partial WeatherForecastSnapshot ToSnapshot(this WeatherForecast weatherForecast);

    // From WeatherForecastSnapshot to WeatherForecast
    public static partial WeatherForecast ToModel(this WeatherForecastSnapshot weatherForecastSnapshot);

    // From WeatherForecastDay to WeatherForecastDaySnapshot
    public static partial WeatherForecastDaySnapshot ToSnapshot(this WeatherForecastDay weatherForecastDay);

    // From WeatherForecastDaySnapshot to WeatherForecastDay
    public static partial WeatherForecastDay ToModel(this WeatherForecastDaySnapshot weatherForecastDaySnapshot);

    // From WeatherState to WeatherStateSnapshot
    public static partial WeatherStateSnapshot ToSnapshot(this WeatherState weatherState);

    // From WeatherStateSnapshot to WeatherState
    public static partial WeatherState ToModel(this WeatherStateSnapshot weatherStateSnapshot);

    // From WeatherLocation to WeatherLocationSnapshot
    public static WeatherLocationSnapshot ToSnapshot(this WeatherLocation weatherLocation) =>
        new(weatherLocation.City, weatherLocation.Country, weatherLocation.Latitude, weatherLocation.Longitude);

    // From WeatherLocationSnapshot to WeatherLocation
    public static WeatherLocation ToModel(this WeatherLocationSnapshot weatherLocationSnapshot) =>
        new(weatherLocationSnapshot.City, weatherLocationSnapshot.Country, weatherLocationSnapshot.Latitude,
            weatherLocationSnapshot.Longitude);
}