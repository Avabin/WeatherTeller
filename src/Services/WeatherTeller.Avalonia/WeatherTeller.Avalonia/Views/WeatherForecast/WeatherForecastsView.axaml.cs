using Avalonia.ReactiveUI;
using ReactiveUI;
using WeatherTeller.ViewModels.WeatherForecast;

namespace WeatherTeller.Avalonia.Views.WeatherForecast;

internal partial class WeatherForecastsView : ReactiveUserControl<WeatherForecastsViewModel>
{
    public WeatherForecastsView()
    {
        InitializeComponent();
        
        this.WhenActivated(disposables =>
        {
            // Add activation logic here
        });
    }
}