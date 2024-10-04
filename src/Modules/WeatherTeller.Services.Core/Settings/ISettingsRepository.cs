namespace WeatherTeller.Services.Core.Settings
{
    public interface ISettingsRepository
    {
        Task<Persistence.Models.Settings> GetSettingsAsync();
        Task UpdateSettingsAsync(Func<Persistence.Models.Settings, Persistence.Models.Settings> update);
    }
}