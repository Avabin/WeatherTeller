using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Persistence.Core.Notifications;

public static class ObservableExtensions
{
    public static IObservable<SettingsChangedNotification> WhereApiKeyChanged(
        this IObservable<SettingsChangedNotification> source) =>
        source.WhereFieldChanged<SettingsChangedNotification, SettingsModel, string, string>(x => x.ApiKey);
    
    public static IObservable<SettingsChangedNotification> WhereLocationChanged(
        this IObservable<SettingsChangedNotification> source) =>
        source.WhereFieldChanged<SettingsChangedNotification, SettingsModel, string, SettingsLocation>(x => x.Location);

}