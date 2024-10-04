using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WeatherTeller.ViewModels.Core;
using WeatherTeller.ViewModels.WeatherForecast.CurrentWeather;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal class WeatherForecastsViewModel : ViewModelBase, IActivatableViewModel, IRoutableViewModel
{
    private readonly ILogger<WeatherForecastsViewModel> _logger;

    public IObservableCollection<WeatherForecastDayViewModel> Forecasts { get; } = new ObservableCollectionExtended<WeatherForecastDayViewModel>();
    [Reactive] public CurrentWeatherForecastViewModel CurrentWeather { get; set; }

    public WeatherForecastsViewModel(ILogger<WeatherForecastsViewModel> logger, CurrentWeatherForecastViewModel currentWeather, IWeatherForecastService forecastService, IScreen hostScreen)
    {
        CurrentWeather = currentWeather;
        HostScreen = hostScreen;
        _logger = logger;

        this.WhenActivated(disposables =>
        {
            forecastService.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(Forecasts)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(disposables);
        });
    }

    public ViewModelActivator Activator { get; } = new();
    public string? UrlPathSegment { get; } = "forecasts";
    public IScreen HostScreen { get; }
}