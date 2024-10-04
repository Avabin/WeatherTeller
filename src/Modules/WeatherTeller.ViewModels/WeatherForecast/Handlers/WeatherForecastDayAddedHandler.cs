using MediatR;
using WeatherTeller.Services.Core.WeatherApi.Notifications;

namespace WeatherTeller.ViewModels.WeatherForecast.Handlers;

internal class WeatherForecastDayAddedHandler : INotificationHandler<DaysForecastStateChangedNotification>
{
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastDayAddedHandler(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    public Task Handle(DaysForecastStateChangedNotification notification, CancellationToken cancellationToken)
    {
        _weatherForecastService.AddRange(notification.Forecast);
        return Task.CompletedTask;
    }
}