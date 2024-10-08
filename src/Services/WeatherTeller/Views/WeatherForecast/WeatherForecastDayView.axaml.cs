using Avalonia.ReactiveUI;
using ReactiveUI;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

namespace WeatherTeller.Views.WeatherForecast;

internal partial class WeatherForecastDayView : ReactiveUserControl<WeatherForecastDayViewModel>
{
    public WeatherForecastDayView()
    {
        InitializeComponent();

        this.WhenActivated(disposables => { });
    }
}