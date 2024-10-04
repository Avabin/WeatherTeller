using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

internal interface IWeatherForecastDayViewModelFactory
{
    WeatherForecastDayViewModel Create(WeatherForecastDay day);
}