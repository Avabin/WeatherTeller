using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal interface IWeatherStateViewModelFactory
{
    WeatherStateViewModel Create(WeatherState state);
}