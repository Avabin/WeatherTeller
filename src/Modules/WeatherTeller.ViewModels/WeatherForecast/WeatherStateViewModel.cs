using ReactiveUI.Fody.Helpers;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.ViewModels.Core;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal class WeatherStateViewModel : ViewModelBase
{
    public WeatherStateViewModel(WeatherState state)
    {
        Location = new WeatherLocationViewModel(state.Location);
        Condition = state.Condition;
        TemperatureC = state.TemperatureC;
        TemperatureF = state.TemperatureF;
        Precipitation = state.Precipitation;
        Pressure = state.Pressure;
    }

    [Reactive] public WeatherLocationViewModel Location { get; set; }
    [Reactive] public string Condition { get; set; }
    [Reactive] public double TemperatureC { get; set; }
    [Reactive] public double TemperatureF { get; set; }
    [Reactive] public double Precipitation { get; set; }

    [Reactive] public double Pressure { get; set; }
}

internal class WeatherLocationViewModel : ViewModelBase
{
    public WeatherLocationViewModel(WeatherLocation location)
    {
        City = location.City;
        Country = location.Country;
        Latitude = location.Latitude;
        Longitude = location.Longitude;
    }

    [Reactive] public string City { get; set; }
    [Reactive] public string Country { get; set; }
    [Reactive] public double Latitude { get; set; }
    [Reactive] public double Longitude { get; set; }
}