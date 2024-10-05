using WeatherTeller.Persistence.Models;
using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Services.Settings;

public record SettingsEntityChangedNotification(
    DateTimeOffset Timestamp,
    SettingsModel? Before,
    SettingsModel After)
    : EntityChangedNotification<SettingsModel, string>(Timestamp, Before , After)
{
    // Of static factory method
    public static SettingsEntityChangedNotification Of(SettingsModel after, SettingsModel? before = null) =>
        new(DateTimeOffset.Now, before, after);
}
    
// observable extensions for settings
public static class ObservableExtensions
{
    public static IObservable<SettingsEntityChangedNotification> WhereApiKeyChanged(
        this IObservable<SettingsEntityChangedNotification> source) =>
        source.WhereFieldChanged<SettingsEntityChangedNotification, SettingsModel, string, string>(x => x.ApiKey);
    
    public static IObservable<SettingsEntityChangedNotification> WhereLocationChanged(
        this IObservable<SettingsEntityChangedNotification> source) =>
        source.WhereFieldChanged<SettingsEntityChangedNotification, SettingsModel, string, SettingsLocation>(x => x.Location);

}