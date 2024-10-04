using MediatR;
using WeatherTeller.Services.Core.WeatherApi.Notifications;

namespace WeatherTeller.ViewModels.WeatherForecast.Handlers;

internal class CurrentWeatherStateChangedHandler : INotificationHandler<CurrentWeatherStateChangedNotification>
{
    private readonly IWeatherForecastService _weatherForecastService;

    public CurrentWeatherStateChangedHandler(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    public Task Handle(CurrentWeatherStateChangedNotification notification, CancellationToken cancellationToken)
    {
        _weatherForecastService.SetCurrentWeatherState(notification.State);
        return Task.CompletedTask;
    }
}