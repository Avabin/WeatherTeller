namespace WeatherTeller.Persistence.Models;

public record SettingsEntityChangedNotification(
    DateTimeOffset Timestamp,
    Settings? Before,
    Settings After)
    : EntityChangedNotification<Settings, string>(Timestamp, Before, After)
{
    // Of static factory method
    public static SettingsEntityChangedNotification Of(Settings after, Settings? before = null) =>
        new(DateTimeOffset.Now, before, after);
}
    
// observable extensions for settings
public static class ObservableExtensions
{
    public static IObservable<SettingsEntityChangedNotification> WhereApiKeyChanged(
        this IObservable<SettingsEntityChangedNotification> source) =>
        source.WhereFieldChanged<SettingsEntityChangedNotification, Settings, string, string>(x => x.ApiKey);
    
    public static IObservable<SettingsEntityChangedNotification> WhereLocationChanged(
        this IObservable<SettingsEntityChangedNotification> source) =>
        source.WhereFieldChanged<SettingsEntityChangedNotification, Settings, string, Location>(x => x.Location);

}