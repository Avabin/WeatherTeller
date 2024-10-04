using DynamicData;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal interface IWeatherForecastService
{
    IObservable<IChangeSet<WeatherForecastDayViewModel, DateTimeOffset>> Connect();
    IObservable<WeatherStateViewModel> CurrentWeatherState { get; }
    
    void Add(WeatherForecastDay forecastDay);
    void AddRange(IEnumerable<WeatherForecastDay> forecastDays);
    
    void SetCurrentWeatherState(WeatherState state);
}