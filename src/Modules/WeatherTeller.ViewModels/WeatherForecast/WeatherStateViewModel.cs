using ReactiveUI.Fody.Helpers;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.ViewModels.Core;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal class WeatherStateViewModel : ViewModelBase
{
    [Reactive] public string Location { get; set; }
    [Reactive] public string Condition { get; set; }
    [Reactive] public double TemperatureC { get; set; }
    [Reactive] public double TemperatureF { get; set; }
    [Reactive] public double Precipitation { get; set; }
    
    [Reactive] public double Pressure { get; set; }

    public WeatherStateViewModel(WeatherState state)
    {
        Location = state.Location;
        Condition = state.Condition;
        TemperatureC = state.TemperatureC;
        TemperatureF = state.TemperatureF;
        Precipitation = state.Precipitation;
        Pressure = state.Pressure;
    }
}