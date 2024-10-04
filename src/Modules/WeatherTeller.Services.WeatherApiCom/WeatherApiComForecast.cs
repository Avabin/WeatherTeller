using System.Net.Http.Json;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using WeatherTeller.Services.WeatherApiCom.Interfaces;
using WeatherTeller.Services.WeatherApiCom.Models;

namespace WeatherTeller.Services.WeatherApiCom;

internal class WeatherApiComForecast : IWeatherApiComForecast
{
    private readonly HttpClient _client;
    private readonly IOptions<WeatherApiComClientOptions> _options;
    private readonly ILogger<WeatherApiComForecast> _logger;
    private WeatherApiComClientOptions Options => _options.Value;
    
    
    // retry with exponential backoff when transient errors occur
    // retry on 429 (too many requests) and 5xx (server errors)
    private IAsyncPolicy<HttpResponseMessage> _retryPolicy = Policy
        .HandleResult<HttpResponseMessage>(r => r.StatusCode == System.Net.HttpStatusCode.TooManyRequests || (int)r.StatusCode >= 500)
        .WaitAndRetryForeverAsync(i => TimeSpan.FromSeconds(Math.Max(Math.Pow(2, i), 360)));
    
    private double _latitude = 0;
    private double _longitude = 0;

    public WeatherApiComForecast(HttpClient client, IOptions<WeatherApiComClientOptions> options,
        ILogger<WeatherApiComForecast> logger)
    {
        _client = client;
        _options = options;
        _logger = logger;
    }

    private ISubject<WeatherForecast> _forecast = new ReplaySubject<WeatherForecast>(1);
    public IObservable<WeatherForecast> Forecast => _forecast;

    public async Task SetLocation(double latitude, double longitude)
    {
        _latitude = latitude;
        _longitude = longitude;
    }

    public async Task SetApiKey(string apiKey)
    {
        Options.ApiKey = apiKey;
        _logger.LogInformation("API key changed");
    }

    public async Task Refresh()
    {
        if (string.IsNullOrWhiteSpace(Options.ApiKey))
        {
            _logger.LogWarning("API key must be set before refreshing weather data");
            return;
        }
        if (_latitude == 0 || _longitude == 0)
        {
            _logger.LogWarning("Latitude and longitude must be set before refreshing weather data");
            return;
        }
        var response = await GetForecast(_latitude, _longitude);
        if (response is null)
        {
            return;
        }
        
        _forecast.OnNext(response.Value.WeatherForecast);
    }
    
    private async Task<ForecastResponse?> GetForecast(double latitude, double longitude)
    {
        var response = await _retryPolicy.ExecuteAsync(() => GetForecastResponse(latitude, longitude));
        if (response is null)
        {
            _logger.LogWarning("Failed to get forecast data");
            return null;
        }

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<ForecastResponse>();
        
        _logger.LogWarning("Failed to get forecast data: {StatusCode}", response.StatusCode);
        return null;
    }
    
    private async Task<HttpResponseMessage> GetForecastResponse(double latitude, double longitude) =>
        await _client.GetAsync(
            $"{Options.ForecastEndpoint}?key={Options.ApiKey}&q={latitude},{longitude}&days={Options.DefaultDays}");
}