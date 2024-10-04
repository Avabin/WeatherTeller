using System.Reactive;
using System.Reactive.Linq;
using Commons.ReactiveCommandGenerator.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WeatherTeller.ViewModels.Core;
using WeatherTeller.ViewModels.Settings;
using WeatherTeller.ViewModels.WeatherForecast;

namespace WeatherTeller.ViewModels.Main;

internal partial class MainViewModel(
    Lazy<SettingsViewModel> settingsLazy,
    Lazy<WeatherForecastsViewModel> weatherForecastsLazy
) : ViewModelBase, IScreen
{
    private readonly Lazy<SettingsViewModel> _settingsLazy = settingsLazy;
    private readonly Lazy<WeatherForecastsViewModel> _weatherForecastsLazy = weatherForecastsLazy;
    [Reactive] public string Greeting { get; set; } = "123";

    [ReactiveCommand]
    private async Task GetGreeting()
    {
        await Task.Delay(1000);
        var greeting = "Hello, World!" + DateTime.Now;
        
        Greeting = greeting;
    }
    
    [ReactiveCommand]
    private async Task NavigateToSettings() => await Router.Navigate.Execute(_settingsLazy.Value);
    
    [ReactiveCommand]
    private async Task NavigateToWeatherForecasts() => await Router.Navigate.Execute(_weatherForecastsLazy.Value);

    public RoutingState Router { get; } = new();
}