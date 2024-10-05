using System.Collections.Immutable;
using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.Services.Core.WeatherApi;

public interface IWeatherApi
{
    IObservable<WeatherState> Current { get; }
    IObservable<ImmutableList<WeatherForecastDay>> Days { get; }
    
    Task SetLocation(double latitude, double longitude);
    
    Task Refresh();
}