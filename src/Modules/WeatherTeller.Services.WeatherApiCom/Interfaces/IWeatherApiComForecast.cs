using WeatherTeller.Services.WeatherApiCom.Models;

namespace WeatherTeller.Services.WeatherApiCom.Interfaces;

/// <summary>
/// Interface for Weather API Com Forecast service.
/// </summary>
public interface IWeatherApiComForecast
{
    /// <summary>
    /// Gets the weather forecast as an observable sequence.
    /// </summary>
    internal IObservable<WeatherForecast> Forecast { get; }
    
    /// <summary>
    /// Sets the location for the weather forecast.
    /// </summary>
    /// <param name="latitude">The latitude of the location.</param>
    /// <param name="longitude">The longitude of the location.</param>
    Task SetLocation(double latitude, double longitude);
    
    
    /// <summary>
    /// Sets the API key for accessing the WeatherApiCom API.
    /// </summary>
    /// <param name="apiKey">The API key to use.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetApiKey(string apiKey);

    /// <summary>
    /// Refreshes the weather forecast data.
    /// </summary>
    Task Refresh();
}