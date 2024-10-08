using MediatR;
using Microsoft.Extensions.Hosting;
using WeatherTeller.Persistence.Core;
using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Services.Settings;

internal class LoadSettingsStartupTask : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly ISettingsRepository _repository;

    public LoadSettingsStartupTask(ISettingsRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = await _repository.GetSettingsAsync();
        if (settings is null)
        {
            var userName = Environment.UserName;
            var newSettings = new SettingsModel(userName, null, "");
            await _repository.CreateSettingsAsync(newSettings);
            settings = newSettings;
        }

        await _mediator.Publish(SettingsLoadedNotification.Of(settings), stoppingToken);
    }
}