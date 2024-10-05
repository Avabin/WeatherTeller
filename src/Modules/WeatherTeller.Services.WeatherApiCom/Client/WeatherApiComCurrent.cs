using System.Net;
using System.Net.Http.Json;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using WeatherTeller.Services.WeatherApiCom.Client.Interfaces;
using WeatherTeller.Services.WeatherApiCom.Models;

namespace WeatherTeller.Services.WeatherApiCom.Client;

internal class WeatherApiComCurrent : IWeatherApiComCurrent
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherApiComCurrent> _logger;
    private WeatherApiComClientOptions _clientOptions;

    // retry with exponential backoff when transient errors occur
    // retry on 429 (too many requests) and 5xx (server errors)
    private IAsyncPolicy<HttpResponseMessage> _retryPolicy = Policy
        .HandleResult<HttpResponseMessage>(r =>
            r.StatusCode == HttpStatusCode.TooManyRequests || (int)r.StatusCode >= 500)
        .WaitAndRetryForeverAsync(i => TimeSpan.FromSeconds(Math.Max(Math.Pow(2, i), 360)));

    private double _latitude = 0;
    private double _longitude = 0;

    public WeatherApiComCurrent(HttpClient httpClient,
        IOptions<WeatherApiComClientOptions> options,
        ILogger<WeatherApiComCurrent> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _clientOptions = options.Value;
    }

    private ISubject<WeatherState> _currentWeather = new ReplaySubject<WeatherState>(1);
    public IObservable<WeatherState> CurrentWeather => _currentWeather;
    private ISubject<WeatherLocation> _location = new ReplaySubject<WeatherLocation>(1);
    public IObservable<WeatherLocation> Location => _location;

    public Task SetLocation(double latitude, double longitude)
    {
        _logger.LogInformation("Setting location to {Latitude}, {Longitude}", latitude, longitude);
        _latitude = latitude;
        _longitude = longitude;
        return Task.CompletedTask;
    }

    public Task SetApiKey(string apiKey)
    {
        _clientOptions.ApiKey = apiKey;
        _logger.LogInformation("API key changed");
        return Task.CompletedTask;
    }

    public async Task Refresh()
    {
        if (string.IsNullOrWhiteSpace(_clientOptions.ApiKey))
        {
            _logger.LogWarning("API key must be set before refreshing weather data");
            return;
        }
        if (_latitude == 0 || _longitude == 0)
        {
            _logger.LogWarning("Latitude and longitude must be set before refreshing weather data");
            return;
        }

        _logger.LogInformation("Refreshing weather data");
        var (location, currentWeather) = await GetCurrentWeather(_latitude, _longitude);

        _logger.LogInformation("Received weather data for {Location} ({Latitude}, {Longitude}): {Weather}",
            location.Name, _latitude, _longitude, currentWeather);
        _currentWeather.OnNext(currentWeather);

        _logger.LogInformation("Received location data for {Location} ({Latitude}, {Longitude})", location.Name,
            _latitude, _longitude);
        _location.OnNext(location);
    }

    private async Task<CurrentWeatherResponse> GetCurrentWeather(double latitude, double longitude)
    {
        var response = await _retryPolicy.ExecuteAsync(() => GetCurrentWeatherResponse(latitude, longitude));
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CurrentWeatherResponse>();
    }

    private async Task<HttpResponseMessage> GetCurrentWeatherResponse(double latitude, double longitude) =>
        await _httpClient.GetAsync($"{_clientOptions.CurrentWeatherEndpoint}?key={_clientOptions.ApiKey}&q={latitude},{longitude}");
}