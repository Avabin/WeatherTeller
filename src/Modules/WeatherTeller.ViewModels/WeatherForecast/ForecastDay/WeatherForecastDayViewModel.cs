using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.ViewModels.Core;

namespace WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

internal class WeatherForecastDayViewModel : ViewModelBase
{
    public WeatherForecastDayViewModel(WeatherForecastDay day, IWeatherStateViewModelFactory weatherStateViewModelFactory)
    {
        Date = day.Date;
        State = weatherStateViewModelFactory.Create(day.State);
    }
    
    public DateOnly Date { get; }
    public WeatherStateViewModel State { get; }
}