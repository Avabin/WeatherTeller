using WeatherTeller.Persistence.Models;

namespace WeatherTeller.Persistence;

/// <summary>
/// Represents a data source specifically for settings, providing CRUD operations.
/// </summary>
public interface ISettingsDataSource : IDataSource<Settings, string>
{
}