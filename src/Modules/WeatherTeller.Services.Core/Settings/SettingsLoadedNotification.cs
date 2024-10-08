using MediatR;

namespace WeatherTeller.Services.Core.Settings;

public record SettingsLoadedNotification(
    string ApiKey,
    double Latitude,
    double Longitude
) : INotification
{
    public static SettingsLoadedNotification Of(SettingsModel settings) =>
        new(settings.ApiKey, settings.Location?.Latitude ?? 0, settings.Location?.Longitude ?? 0);
}