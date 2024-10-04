using MediatR;
using WeatherTeller.Persistence.Models;
using WeatherTeller.Services.WeatherApiCom.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom.Handlers;

public class WeatherApiComSettingsChangedNotificationHandler : INotificationHandler<SettingsEntityChangedNotification>
{
    private readonly IWeatherApiComClient _weatherApiComClient;

    public WeatherApiComSettingsChangedNotificationHandler(IWeatherApiComClient weatherApiComClient)
    {
        _weatherApiComClient = weatherApiComClient;
    }

    public async Task Handle(SettingsEntityChangedNotification notification, CancellationToken cancellationToken)
    {
        await _weatherApiComClient.SetSettings(notification.After.ApiKey!, notification.After.Location!.Latitude, notification.After.Location.Longitude);
    }
}