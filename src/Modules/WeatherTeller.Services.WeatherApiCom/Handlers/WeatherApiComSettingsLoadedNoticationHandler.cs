using MediatR;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.WeatherApiCom.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom.Handlers;

public class WeatherApiComSettingsLoadedNotificationHandler : INotificationHandler<SettingsLoadedNotification>
{
    private readonly IWeatherApiComClient _weatherApiComClient;

    public WeatherApiComSettingsLoadedNotificationHandler(IWeatherApiComClient weatherApiComClient)
    {
        _weatherApiComClient = weatherApiComClient;
    }

    public async Task Handle(SettingsLoadedNotification notification, CancellationToken cancellationToken)
    {
        await _weatherApiComClient.SetSettings(notification.ApiKey, notification.Latitude, notification.Longitude);
    }
}
