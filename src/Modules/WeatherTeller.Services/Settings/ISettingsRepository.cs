using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Services.Settings;

internal interface ISettingsRepository
{
    Task<SettingsModel?> GetSettingsAsync();
    
    Task CreateSettingsAsync(SettingsModel settings);
    Task UpdateSettingsAsync(Func<SettingsModel, SettingsModel> update);
}