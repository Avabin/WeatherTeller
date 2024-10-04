using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

internal class WeatherForecastDayViewModelFactory : IWeatherForecastDayViewModelFactory
{
    private readonly IWeatherStateViewModelFactory _weatherStateViewModelFactory;

    public WeatherForecastDayViewModelFactory(IWeatherStateViewModelFactory weatherStateViewModelFactory)
    {
        _weatherStateViewModelFactory = weatherStateViewModelFactory;
    }

    public WeatherForecastDayViewModel Create(WeatherForecastDay day) => new(day, _weatherStateViewModelFactory);
}