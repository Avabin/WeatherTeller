using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WeatherTeller.ViewModels.Core;

namespace WeatherTeller.ViewModels.WeatherForecast.CurrentWeather;

internal class CurrentWeatherForecastViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly ILogger<CurrentWeatherForecastViewModel> _logger;
    private readonly IWeatherForecastService _forecastService;

    public CurrentWeatherForecastViewModel(ILogger<CurrentWeatherForecastViewModel> logger,IWeatherForecastService forecastService)
    {
        _logger = logger;
        _forecastService = forecastService;
        
        this.WhenActivated(disposables =>
        {
            _forecastService.CurrentWeatherState
                .BindTo(this, x => x.WeatherState)
                .DisposeWith(disposables);
        });
    }
    
    [Reactive]
    public WeatherStateViewModel? WeatherState { get; set; }

    public ViewModelActivator Activator { get; } = new();
}