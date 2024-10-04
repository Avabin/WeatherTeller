using Microsoft.Extensions.Logging;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.ViewModels.Core;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal class WeatherForecastViewModel: ViewModelBase, IObserver<WeatherForecastDay>
{
    private readonly ILogger<WeatherForecastViewModel> _logger;

    public WeatherForecastViewModel(ILogger<WeatherForecastViewModel> logger, IWeatherForecastDayViewModelFactory weatherForecastDayViewModelFactory)
    {
        _logger = logger;
    }
    public void OnCompleted() => _logger.LogInformation("Weather forecast completed");

    public void OnError(Exception error) => _logger.LogError(error, "Error in weather forecast");

    public void OnNext(WeatherForecastDay value)
    {
        _logger.LogInformation("Weather forecast updated");
    }
}