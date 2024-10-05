using MediatR;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.WeatherApiCom.Client.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom.Handlers;

internal class WeatherApiComSettingsLoadedNotificationHandler(IWeatherApiComClient weatherApiComClient)
    : INotificationHandler<SettingsLoadedNotification>
{
    public async Task Handle(SettingsLoadedNotification notification, CancellationToken cancellationToken) => 
        await weatherApiComClient.SetSettings(notification.ApiKey, notification.Latitude, notification.Longitude);
}
