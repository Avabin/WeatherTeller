using MediatR;
using Microsoft.Extensions.Logging;
using WeatherTeller.Persistence.Core.Notifications;
using WeatherTeller.Services.WeatherApiCom.Client.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom.Handlers;

internal class WeatherApiComSettingsChangedNotificationHandler(IWeatherApiComClient weatherApiComClient, ILogger<WeatherApiComSettingsChangedNotificationHandler> logger)
    : INotificationHandler<SettingsChangedNotification>
{
    private readonly IWeatherApiComClient _weatherApiComClient = weatherApiComClient;
    private readonly ILogger<WeatherApiComSettingsChangedNotificationHandler> _logger = logger;

    public async Task Handle(SettingsChangedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling settings change notification for WeatherApiCom");
        var settings = notification.After;
        await _weatherApiComClient.SetSettings(settings.ApiKey, settings.Location?.Latitude ?? 0, settings.Location?.Longitude ?? 0);
    }
        
}