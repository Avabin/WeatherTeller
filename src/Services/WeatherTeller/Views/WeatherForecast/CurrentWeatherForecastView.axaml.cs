using Avalonia.ReactiveUI;
using ReactiveUI;
using WeatherTeller.ViewModels.WeatherForecast.CurrentWeather;

namespace WeatherTeller.Views.WeatherForecast;

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