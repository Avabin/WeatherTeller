using System.Reactive.Disposables;
using System.Reactive.Linq;
using Commons.ReactiveCommandGenerator.Core;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using WeatherTeller.ViewModels.Core;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal partial class WeatherForecastsViewModel : ViewModelBase, IActivatableViewModel, IRoutableViewModel
{
    private readonly IWeatherForecastService _forecastService;
    private readonly ILogger<WeatherForecastsViewModel> _logger;

    private readonly IObservable<DateTimeOffset> _currentTime;

    private readonly TimeProvider _timeProvider;

    public WeatherForecastsViewModel(ILogger<WeatherForecastsViewModel> logger, IWeatherForecastService forecastService,
        IScreen hostScreen, TimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;

        _currentTime = Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(_ => _timeProvider.GetLocalNow())
            .Publish()
            .RefCount();

        HostScreen = hostScreen;
        _logger = logger;
        _forecastService = forecastService;

        this.WhenActivated(disposables =>
        {
            forecastService.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(x => _logger.LogInformation("Weather forecasts updated"))
                .Bind(Forecasts)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(disposables);
        });
    }

    public IObservableCollection<WeatherForecastDayViewModel> Forecasts { get; } =
        new ObservableCollectionExtended<WeatherForecastDayViewModel>();

    public IObservable<DateTimeOffset> CurrentTime =>
        _currentTime.ObserveOn(RxApp.MainThreadScheduler).SubscribeOn(RxApp.MainThreadScheduler);

    public ViewModelActivator Activator { get; } = new();
    public string? UrlPathSegment { get; } = "forecasts";
    public IScreen HostScreen { get; }

    [ReactiveCommand]
    private async Task Refresh() => await _forecastService.Refresh();
}