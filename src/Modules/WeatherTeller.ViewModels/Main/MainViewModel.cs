using System.Reactive.Linq;
using Commons.ReactiveCommandGenerator.Core;
using MediatR;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WeatherTeller.Services.Core.Settings.Requests;
using WeatherTeller.ViewModels.Configuration;
using WeatherTeller.ViewModels.Core;
using WeatherTeller.ViewModels.Settings;
using WeatherTeller.ViewModels.WeatherForecast;

namespace WeatherTeller.ViewModels.Main;

internal partial class MainViewModel(
    Lazy<SettingsViewModel> settingsLazy,
    Lazy<WeatherForecastsViewModel> weatherForecastsLazy,
    ConfigurationWizardViewModelFactory wizardViewModelFactory,
    IMediator mediator
) : ViewModelBase, IScreen
{
    private readonly Lazy<SettingsViewModel> _settingsLazy = settingsLazy;
    private readonly Lazy<WeatherForecastsViewModel> _weatherForecastsLazy = weatherForecastsLazy;
    private readonly ConfigurationWizardViewModelFactory _wizardViewModelFactory = wizardViewModelFactory;
    private readonly IMediator _mediator = mediator;


    [ReactiveCommand]
    private async Task CheckSettings()
    {
        var settings = await _mediator.Send(new GetSettingsRequest());
        var hasApiKey = !string.IsNullOrWhiteSpace(settings?.ApiKey);
        var hasLocation = settings?.Location is not null && !string.IsNullOrWhiteSpace(settings.Location.Name) && settings.Location.Latitude != 0 && settings.Location.Longitude != 0;
        
        var needsConfiguration = !hasApiKey || !hasLocation;
        if (!needsConfiguration)
        {
            await NavigateToWeatherForecasts();
            return;
        }
        var wizard = _wizardViewModelFactory.Create(!hasLocation, !hasApiKey);
        
        Router.NavigationStack.Add(_weatherForecastsLazy.Value);
        await Router.Navigate.Execute(wizard);
    }
    
    [ReactiveCommand]
    private async Task NavigateToSettings() => await Router.Navigate.Execute(_settingsLazy.Value).ObserveOn(RxApp.MainThreadScheduler);
    
    [ReactiveCommand]
    private async Task NavigateToWeatherForecasts() => await Router.Navigate.Execute(_weatherForecastsLazy.Value).ObserveOn(RxApp.MainThreadScheduler);

    public RoutingState Router { get; } = new();
}