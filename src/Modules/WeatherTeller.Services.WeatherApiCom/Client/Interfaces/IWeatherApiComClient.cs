namespace WeatherTeller.Services.WeatherApiCom.Client.Interfaces;

/// <summary>
/// Interface for interacting with the WeatherApiCom service.
/// </summary>
internal interface IWeatherApiComClient
{
    /// <summary>
    /// Gets the current weather data.
    /// </summary>
    IWeatherApiComCurrent Current { get; }

    /// <summary>
    /// Gets the weather forecast data.
    /// </summary>
    IWeatherApiComForecast Forecast { get; }

    /// <summary>
    /// Sets the location for the weather data.
    /// </summary>
    /// <param name="latitude">The latitude of the location.</param>
    /// <param name="longitude">The longitude of the location.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetLocation(double latitude, double longitude)
    {
        await Current.SetLocation(latitude, longitude);
        await Forecast.SetLocation(latitude, longitude);
    }

    /// <summary>
    /// Refreshes the weather data.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Refresh()
    {
        await Current.Refresh();
        await Forecast.Refresh();
    }

    Task SetSettings(string apiKey, double latitude, double longitude);
}