namespace WeatherTeller.Persistence.Settings;

/// <summary>
/// Represents a data source specifically for settings, providing CRUD operations.
/// </summary>
internal interface ISettingsDataSource : IDataSource<Settings, string>
{
}