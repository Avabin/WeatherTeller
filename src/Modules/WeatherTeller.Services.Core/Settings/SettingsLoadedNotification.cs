using MediatR;

namespace WeatherTeller.Services.Core.Settings
{
    public record SettingsLoadedNotification(
        string ApiKey,
        double Latitude,
        double Longitude
    ) : INotification
    {
    
    }
}