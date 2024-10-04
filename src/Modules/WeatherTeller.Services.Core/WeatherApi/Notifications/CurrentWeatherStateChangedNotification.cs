using MediatR;
using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.Services.Core.WeatherApi.Notifications
{
    public record CurrentWeatherStateChangedNotification(WeatherState State) : INotification
    {
    
    }
}