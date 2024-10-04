using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using WeatherTeller.Persistence;
using WeatherTeller.Persistence.Models;
using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Services.Settings;

public class SettingsRepository : ISettingsRepository, IDisposable
{
    private IDisposable _settingsChangedSubscription;
    private readonly IMediator _mediator;
    private readonly ISettingsDataSource _settingsDataSource;
    private readonly ILogger<SettingsRepository> _logger;

    private ISubject<SettingsEntityChangedNotification> _settingsChanged = new ReplaySubject<SettingsEntityChangedNotification>(1);
    
    private void PublishSettingsChanged(Persistence.Models.Settings? oldSettings, Persistence.Models.Settings newSettings) =>
        _settingsChanged.OnNext(new SettingsEntityChangedNotification(DateTimeOffset.Now, oldSettings, newSettings));

    public SettingsRepository(IMediator mediator,
        ISettingsDataSource settingsDataSource, 
        ILogger<SettingsRepository> logger)
    {
        _mediator = mediator;
        _settingsDataSource = settingsDataSource;
        _logger = logger;
        
        _settingsChangedSubscription = ObserveSettingsChanged
            .DistinctUntilChanged()
            .SelectMany(x => _mediator.Publish(x).ToObservable())
            .Subscribe();
    }

    public IObservable<SettingsEntityChangedNotification> ObserveSettingsChanged =>
        _settingsChanged.AsObservable();

    public async Task<Persistence.Models.Settings> GetSettingsAsync()
    {
        var currentUserName = Environment.UserName;
        var settings = await _settingsDataSource.GetById(currentUserName)
            .ObserveOn(TaskPoolScheduler.Default)
            .FirstOrDefaultAsync();
        if (settings is not null)
        {
            return settings;
        }
        
        _logger.LogInformation("Settings not found for user {UserName}", currentUserName);
        var newSettings = new Persistence.Models.Settings
        {
            Id = new Id<string>(currentUserName),
            Location = new Location("New York", 40.7128, -74.0060),
        };
            
        await _settingsDataSource.Add(newSettings);
        _logger.LogInformation("Settings created for user {UserName}", currentUserName);

        return newSettings;
    }
    
    public async Task UpdateSettingsAsync(Func<Persistence.Models.Settings, Persistence.Models.Settings> update)
    {
        var currentUserName = Environment.UserName;
        var settings = await _settingsDataSource.GetById(currentUserName)
            .ObserveOn(TaskPoolScheduler.Default)
            .FirstOrDefaultAsync();
        if (settings is null)
        {
            _logger.LogWarning("Settings not found for user {UserName}", currentUserName);
            return;
        }
        
        var updatedSettings = update(settings);
        await _settingsDataSource.ReplaceOne(currentUserName, updatedSettings);
        PublishSettingsChanged(settings, updatedSettings);
    }

    public void Dispose() => _settingsChangedSubscription.Dispose();
}