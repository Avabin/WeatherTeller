using System.Collections.Immutable;
using System.Reactive.Linq;
using Commons.ReactiveCommandGenerator.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using WeatherTeller.Services.Core.Settings.Requests;
using WeatherTeller.Services.Core.WeatherApi.Notifications;
using WeatherTeller.Services.Core.WeatherForecasts.Requests;
using WeatherTeller.ViewModels.Configuration;
using WeatherTeller.ViewModels.Core;
using WeatherTeller.ViewModels.Settings;
using WeatherTeller.ViewModels.WeatherForecast;

namespace WeatherTeller.ViewModels.Main;

internal partial class MainViewModel(
    Lazy<SettingsViewModel> settingsLazy,
    Lazy<WeatherForecastsViewModel> weatherForecastsLazy,
    ConfigurationWizardViewModelFactory wizardViewModelFactory,
    IMediator mediator,
    ILogger<MainViewModel> logger
) : ViewModelBase, IScreen
{
    private readonly ILogger<MainViewModel> _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly Lazy<SettingsViewModel> _settingsLazy = settingsLazy;
    private readonly Lazy<WeatherForecastsViewModel> _weatherForecastsLazy = weatherForecastsLazy;
    private readonly ConfigurationWizardViewModelFactory _wizardViewModelFactory = wizardViewModelFactory;

    public RoutingState Router { get; } = new();


    [ReactiveCommand]
    private async Task CheckSettings()
    {
        var settings = await _mediator.Send(new GetSettingsRequest());
        var hasApiKey = !string.IsNullOrWhiteSpace(settings?.ApiKey);
        var hasLocation = settings?.Location is not null && settings.Location.Latitude != 0 &&
                          settings.Location.Longitude != 0;

        var needsConfiguration = !hasApiKey || !hasLocation;
        if (!needsConfiguration)
        {
            await NavigateToWeatherForecasts();
            return;
        }

        _logger.LogInformation("Settings are not configured, showing configuration wizard");
        var wizard = _wizardViewModelFactory.Create(!hasLocation, !hasApiKey);

        Router.NavigationStack.Add(_weatherForecastsLazy.Value);
        await Router.Navigate.Execute(wizard);
    }

    [ReactiveCommand]
    private async Task LoadWeatherForecasts()
    {
        var forecast = await _mediator.Send(new GetLatestWeatherForecast());
        if (forecast is null)
        {
            _logger.LogInformation("No weather forecast found, skipping loading");
            return;
        }

        var days = forecast.Days.OrderBy(x => x.Date).ToList();
        // delete all forecasts that are in the past
        days.RemoveAll(x => x.Date < DateOnly.FromDateTime(DateTime.Now));

        if (days.Count != 0)
            await _mediator.Publish(new DaysForecastStateChangedNotification(days.ToImmutableList()));

        // if there are no forecasts left, send request to refresh
        if (days.Count == 0)
            await _mediator.Send(new RefreshWeatherForecastCommand());
    }

    [ReactiveCommand]
    private async Task NavigateToSettings()
    {
        _logger.LogInformation("Navigating to settings");
        await Router.Navigate.Execute(_settingsLazy.Value).ObserveOn(RxApp.MainThreadScheduler);
    }

    [ReactiveCommand]
    private async Task NavigateToWeatherForecasts()
    {
        _logger.LogInformation("Navigating to weather forecasts");
        await Router.Navigate.Execute(_weatherForecastsLazy.Value).ObserveOn(RxApp.MainThreadScheduler);
    }
}