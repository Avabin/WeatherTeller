using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherTeller.Persistence.Models;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.WeatherApiCom.Interfaces;
using WeatherTeller.Services.WeatherApiCom.Models;

namespace WeatherTeller.Services.WeatherApiCom;

internal class WeatherApiComClient : IWeatherApiComClient, IDisposable
{
    private readonly ILogger<WeatherApiComClient> _logger;

    private CompositeDisposable _disposables = new();

    public WeatherApiComClient(IWeatherApiComCurrent currentWeather, IWeatherApiComForecast forecast, ILogger<WeatherApiComClient> logger)
    {
        _logger = logger;
        Current = currentWeather;
        Forecast = forecast;
    }

    public IWeatherApiComCurrent Current { get; }

    public IWeatherApiComForecast Forecast { get; }

    public async Task SetSettings(string apiKey, double latitude, double longitude)
    {
        _logger.LogInformation("Setting API key and location for WeatherApiCom");
        
        await Current.SetApiKey(apiKey);
        await Forecast.SetApiKey(apiKey);
        
        await Current.SetLocation(latitude, longitude);
        await Forecast.SetLocation(latitude, longitude);
        
        await Current.Refresh();
        await Forecast.Refresh();
    }

    public void Dispose() => _disposables.Dispose();
}

