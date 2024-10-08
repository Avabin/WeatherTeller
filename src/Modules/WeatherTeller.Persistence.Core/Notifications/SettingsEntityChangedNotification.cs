using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Persistence.Core.Notifications;

public record SettingsChangedNotification(
    DateTimeOffset Timestamp,
    SettingsModel? Before,
    SettingsModel After)
    : EntityChangedNotification<SettingsModel, string>(Timestamp, Before, After)
{
    // Of static factory method
    public static SettingsChangedNotification Of(SettingsModel after, SettingsModel? before = null) =>
        new(DateTimeOffset.Now, before, after);
}

// observable extensions for settings