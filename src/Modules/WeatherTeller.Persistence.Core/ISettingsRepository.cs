using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Persistence.Core;

public interface ISettingsRepository
{
    Task<SettingsModel?> GetSettingsAsync();

    Task CreateSettingsAsync(SettingsModel settings);
    Task UpdateSettingsAsync(Func<SettingsModel, SettingsModel> update);
}