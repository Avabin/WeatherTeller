using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal class WeatherStateViewModelFactory : IWeatherStateViewModelFactory
{
    public WeatherStateViewModel Create(WeatherState state) => new(state);
}