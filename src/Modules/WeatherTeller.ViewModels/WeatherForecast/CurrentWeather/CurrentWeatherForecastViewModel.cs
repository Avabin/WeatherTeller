using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WeatherTeller.ViewModels.Core;

namespace WeatherTeller.ViewModels.WeatherForecast.CurrentWeather;

internal class CurrentWeatherForecastViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly ILogger<CurrentWeatherForecastViewModel> _logger;

    public IObservable<DateTimeOffset> CurrentTime { get; }

    public CurrentWeatherForecastViewModel(ILogger<CurrentWeatherForecastViewModel> logger,IWeatherForecastService forecastService, TimeProvider? timeProvider = null)
    {
        _logger = logger;
        var timeProvider1 = timeProvider ?? TimeProvider.System;
        
        CurrentTime = Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(_ => timeProvider1.GetLocalNow())
            .Publish()
            .RefCount();

        this.WhenActivated(disposables =>
        {
            forecastService.CurrentWeatherState
                .BindTo(this, x => x.WeatherState)
                .DisposeWith(disposables);
        });
    }
    
    [Reactive]
    public WeatherStateViewModel? WeatherState { get; set; }

    public ViewModelActivator Activator { get; } = new();
}