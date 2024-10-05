using MediatR;
using Microsoft.Extensions.Hosting;

namespace WeatherTeller.Services.Settings;

internal class LoadSettingsStartupTask : BackgroundService
{
    private readonly ISettingsRepository _repository;
    private readonly IMediator _mediator;

    public LoadSettingsStartupTask(ISettingsRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = await _repository.GetSettingsAsync();
        await _mediator.Publish(SettingsEntityChangedNotification.Of(settings), stoppingToken);
    }
}