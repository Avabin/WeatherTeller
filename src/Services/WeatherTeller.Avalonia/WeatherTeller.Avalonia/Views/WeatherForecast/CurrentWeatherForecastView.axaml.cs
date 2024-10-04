using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using WeatherTeller.ViewModels.WeatherForecast.CurrentWeather;

namespace WeatherTeller.Avalonia.Views.WeatherForecast;

internal partial class CurrentWeatherForecastView : ReactiveUserControl<CurrentWeatherForecastViewModel>
{
    public CurrentWeatherForecastView()
    {
        InitializeComponent();
        
        this.WhenActivated(disposables =>
        {
            // Add activation logic here
        });
    }
}