using MediatR;
using Microsoft.Extensions.Logging;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.WeatherApiCom.Client.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom.Handlers;

internal class WeatherApiComSettingsLoadedNotificationHandler(
    IWeatherApiComClient weatherApiComClient,
    ILogger<WeatherApiComSettingsLoadedNotificationHandler> logger)
    : INotificationHandler<SettingsLoadedNotification>
{
    private readonly ILogger<WeatherApiComSettingsLoadedNotificationHandler> _logger = logger;
    private readonly IWeatherApiComClient _weatherApiComClient = weatherApiComClient;

    public async Task Handle(SettingsLoadedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling settings loaded notification for WeatherApiCom");
        await _weatherApiComClient.SetSettings(notification.ApiKey, notification.Latitude, notification.Longitude);
    }
}