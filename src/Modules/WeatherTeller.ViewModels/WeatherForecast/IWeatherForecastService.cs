using DynamicData;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal interface IWeatherForecastService
{
    IObservable<IChangeSet<WeatherForecastDayViewModel, DateOnly>> Connect();

    void Add(WeatherForecastDay forecastDay);
    void AddRange(IEnumerable<WeatherForecastDay> forecastDays);

    Task Refresh();
}