using MediatR;
using WeatherTeller.Services.Core.WeatherApi.Notifications;

namespace WeatherTeller.ViewModels.WeatherForecast.Handlers;

internal class DaysForecastChangedHandler(IWeatherForecastService weatherForecastService)
    : INotificationHandler<DaysForecastStateChangedNotification>
{
    public Task Handle(DaysForecastStateChangedNotification notification, CancellationToken cancellationToken)
    {
        weatherForecastService.AddRange(notification.Forecast);
        return Task.CompletedTask;
    }
}