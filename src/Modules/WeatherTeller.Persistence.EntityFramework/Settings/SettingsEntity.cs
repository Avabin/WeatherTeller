using Riok.Mapperly.Abstractions;
using WeatherTeller.Persistence.Models;
using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Persistence.EntityFramework.Settings;

internal record SettingsEntity : IHasId<string>
{
    public LocationEntity? Location { get; set; } = null;
    public string? ApiKey { get; set; } = null;
    public string Id { get; set; } = string.Empty;
}

internal record LocationEntity
{
    public string City { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

[Mapper]
internal static partial class EntityMapper
{
    public static partial Persistence.Settings.Settings ToPersistence(this SettingsEntity entity);

    public static partial SettingsEntity ToEntity(this Persistence.Settings.Settings model);

    // from SettingsEntity to SettingsModel
    public static partial SettingsModel ToSettingsModel(this SettingsEntity entity);

    // from SettingsModel to SettingsEntity
    public static partial SettingsEntity ToEntity(this SettingsModel model);

    public static partial Location ToPersistence(this LocationEntity entity);

    public static partial LocationEntity ToEntity(this Location model);

    // from LocationEntity to SettingsLocation
    public static partial SettingsLocation ToModel(this LocationEntity entity);

    // from SettingsLocation to LocationEntity
    public static partial LocationEntity ToEntity(this SettingsLocation model);
}