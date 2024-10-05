using MediatR;
using WeatherTeller.Services.Settings;
using WeatherTeller.Services.WeatherApiCom.Client.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom.Handlers;

internal class WeatherApiComSettingsChangedNotificationHandler(IWeatherApiComClient weatherApiComClient)
    : INotificationHandler<SettingsEntityChangedNotification>
{
    public async Task Handle(SettingsEntityChangedNotification notification, CancellationToken cancellationToken) =>
        await weatherApiComClient.SetSettings(notification.After.ApiKey!, notification.After.Location?.Latitude ?? 0,
            notification.After.Location?.Longitude ?? 0);
}