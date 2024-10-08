using System.Reactive.Concurrency;
using System.Reactive.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using Riok.Mapperly.Abstractions;
using WeatherTeller.Persistence.Core;
using WeatherTeller.Persistence.Core.Notifications;
using WeatherTeller.Persistence.Models;
using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Persistence.Settings;

internal class SettingsRepository : ISettingsRepository
{
    private readonly ILogger<SettingsRepository> _logger;
    private readonly IMediator _mediator;
    private readonly ISettingsDataSource _settingsDataSource;


    public SettingsRepository(IMediator mediator,
        ISettingsDataSource settingsDataSource,
        ILogger<SettingsRepository> logger)
    {
        _mediator = mediator;
        _settingsDataSource = settingsDataSource;
        _logger = logger;
    }

    public async Task<SettingsModel?> GetSettingsAsync()
    {
        _logger.LogInformation("Getting settings for user {UserName}", Environment.UserName);
        var currentUserName = Environment.UserName;
        var settings = await _settingsDataSource.GetById(currentUserName)
            .ObserveOn(TaskPoolScheduler.Default)
            .FirstOrDefaultAsync();
        if (settings is not null)
            return settings.ToSettingsModel();
        _logger.LogWarning("Settings not found for user {UserName}", currentUserName);
        return null;
    }

    public async Task CreateSettingsAsync(SettingsModel settings)
    {
        _logger.LogInformation("Creating settings for user {UserName}", Environment.UserName);
        await _settingsDataSource.Add(settings.ToPersistenceModel());
        await _mediator.Publish(SettingsChangedNotification.Of(settings));
    }

    public async Task UpdateSettingsAsync(Func<SettingsModel, SettingsModel> update)
    {
        _logger.LogInformation("Updating settings for user {UserName}", Environment.UserName);
        var currentUserName = Environment.UserName;
        var exists = await _settingsDataSource.Contains(currentUserName);
        if (!exists)
        {
            _logger.LogWarning("Settings not found for user {UserName}", currentUserName);
            return;
        }

        var settings = await _settingsDataSource.GetById(currentUserName);
        var settingsModel = settings!.ToSettingsModel();
        var updatedModel = update(settingsModel);
        var updatedSettings = updatedModel.ToPersistenceModel();
        await _settingsDataSource.ReplaceOne(currentUserName, updatedSettings);

        _logger.LogDebug("Settings updated for user {UserName}", currentUserName);
        await _mediator.Publish(SettingsChangedNotification.Of(updatedModel, settingsModel));
    }
}

[Mapper]
public static partial class SettingsRepositoryMapper
{
    internal static SettingsModel ToSettingsModel(this Settings settings) =>
        new(settings.Id, settings.Location?.ToSettingsLocation(), settings.ApiKey ?? string.Empty);

    public static Settings ToPersistenceModel(this SettingsModel settings) =>
        new()
        {
            Id = settings.Id,
            Location = settings.Location?.ToLocation(),
            ApiKey = settings.ApiKey
        };

    // From Location to SettingsLocation
    public static SettingsLocation ToSettingsLocation(this Location location) =>
        new(location.City, location.Latitude, location.Longitude);

    // From SettingsLocation to Location
    public static Location ToLocation(this SettingsLocation location) =>
        new(location.City, location.Latitude, location.Longitude);
}