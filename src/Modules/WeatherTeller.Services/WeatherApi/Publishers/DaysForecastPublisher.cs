using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.Services.Core.WeatherApi.Notifications;

namespace WeatherTeller.Services.WeatherApi.Publishers;

internal class DaysForecastPublisher : IHostedService
{
    private readonly IWeatherApi _weatherApi;
    private readonly IMediator _mediator;
    private readonly ILogger<DaysForecastPublisher> _logger;
    private IDisposable? _subscription;

    public DaysForecastPublisher(IWeatherApi weatherApi, IMediator mediator, ILogger<DaysForecastPublisher> logger)
    {
        _weatherApi = weatherApi;
        _mediator = mediator;
        _logger = logger;
    }

    private async Task Publish(ImmutableList<WeatherForecastDay> forecast)
    {
        _logger.LogDebug("Publishing days forecast");
        var daysCount = forecast.Count;
        _logger.LogInformation("Publishing days forecast with {DaysCount} days", daysCount);
        await _mediator.Publish(new DaysForecastStateChangedNotification(forecast));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting days forecast publisher");
        _subscription = _weatherApi.Days.Select(x => Publish(x).ToObservable()).Concat().Subscribe();
        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping days forecast publisher");
        _subscription?.Dispose();
        return Task.CompletedTask;
    }
}