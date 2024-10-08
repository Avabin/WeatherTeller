using WeatherTeller.Services.WeatherApiCom.Models;

namespace WeatherTeller.Services.WeatherApiCom.Client.Interfaces;

/// <summary>
/// Interface for accessing current weather data from WeatherApiCom.
/// </summary>
internal interface IWeatherApiComCurrent
{
    /// <summary>
    /// Gets an observable stream of the current location.
    /// </summary>
    internal IObservable<WeatherLocation> Location { get; }

    /// <summary>
    /// Sets the location for retrieving weather data.
    /// </summary>
    /// <param name="latitude">The latitude of the location.</param>
    /// <param name="longitude">The longitude of the location.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetLocation(double latitude, double longitude);

    /// <summary>
    /// Sets the API key for accessing the WeatherApiCom API.
    /// </summary>
    /// <param name="apiKey">The API key to use.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetApiKey(string apiKey);

    /// <summary>
    /// Refreshes the current weather data.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Refresh();
}