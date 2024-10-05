using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Riok.Mapperly.Abstractions;
using WeatherTeller.Persistence;
using WeatherTeller.Persistence.Models;
using WeatherTeller.Persistence.Settings;
using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Services.Settings;

internal class SettingsRepository : ISettingsRepository
{
    private readonly IMediator _mediator;
    private readonly ISettingsDataSource _settingsDataSource;
    private readonly ILogger<SettingsRepository> _logger;
        

    public SettingsRepository(IMediator mediator,
        ISettingsDataSource settingsDataSource, 
        ILogger<SettingsRepository> logger)
    {
        _mediator = mediator;
        _settingsDataSource = settingsDataSource;
        _logger = logger;
    }

    public async Task<SettingsModel> GetSettingsAsync()
    {
        var currentUserName = Environment.UserName;
        var settings = await _settingsDataSource.GetById(currentUserName)
            .ObserveOn(TaskPoolScheduler.Default)
            .FirstOrDefaultAsync();
        if (settings is not null)
        {
            return settings.ToSettingsModel();
        }
        
        _logger.LogInformation("Settings not found for user {UserName}", currentUserName);
        var newSettings = new Persistence.Settings.Settings
        {
            Id = new Id<string>(currentUserName),
            Location = new Location("New York", 40.7128, -74.0060),
        };
            
        await _settingsDataSource.Add(newSettings);
        _logger.LogInformation("Settings created for user {UserName}", currentUserName);

        return newSettings.ToSettingsModel();
    }
    
    public async Task UpdateSettingsAsync(Func<SettingsModel, SettingsModel> update)
    {
        var currentUserName = Environment.UserName;
        var settings = await _settingsDataSource.GetById(currentUserName);
        if (settings is null)
        {
            _logger.LogWarning("Settings not found for user {UserName}", currentUserName);
            return;
        }

        var settingsModel = settings.ToSettingsModel();
        var updatedModel = update(settingsModel);
        var updatedSettings = updatedModel.ToPersistenceModel();
        await _settingsDataSource.ReplaceOne(currentUserName, updatedSettings);
        
        _logger.LogInformation("Settings updated for user {UserName}", currentUserName);
        await _mediator.Publish(SettingsEntityChangedNotification.Of(updatedModel, settingsModel));
    }

}

[Mapper]
public static partial class SettingsRepositoryMapper
{
    internal static SettingsModel ToSettingsModel(this Persistence.Settings.Settings settings) =>
        new(settings.Id,Location: settings.Location?.ToSettingsLocation(), settings.ApiKey ?? string.Empty);

    public static Persistence.Settings.Settings ToPersistenceModel(this SettingsModel settings) =>
        new()
        {
            Id = settings.Id,
            Location = settings.Location?.ToLocation(),
            ApiKey = settings.ApiKey
        };

    // From Location to SettingsLocation
    public static SettingsLocation ToSettingsLocation(this Location location) => new(location.Name, location.Latitude, location.Longitude);
    
    // From SettingsLocation to Location
    public static Location ToLocation(this SettingsLocation location) => new(location.Name, location.Latitude, location.Longitude);
}