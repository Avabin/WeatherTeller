using MediatR;

namespace WeatherTeller.Persistence.Core.Notifications;

public record EntityChangedNotification<T, TId>(DateTimeOffset Timestamp, T? Before, T After)
    : INotification
{
}