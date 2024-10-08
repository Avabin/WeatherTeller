using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.Services.Core.WeatherApi.Notifications;

namespace WeatherTeller.Services.WeatherApi.Publishers;

internal class WeatherForecastPublisher : IHostedService
{
    private readonly IWeatherApi _weatherApi;
    private readonly IMediator _mediator;
    private readonly ILogger<WeatherForecastPublisher> _logger;
    private IDisposable? _subscription;

    public WeatherForecastPublisher(IWeatherApi weatherApi, IMediator mediator, ILogger<WeatherForecastPublisher> logger)
    {
        _weatherApi = weatherApi;
        _mediator = mediator;
        _logger = logger;
    }

    private async Task Publish(WeatherForecastDay state)
    {
        _logger.LogDebug("Publishing current weather state");
        await _mediator.Publish(new CurrentWeatherStateChangedNotification(state.State));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting current weather state publisher");
        _subscription = _weatherApi.Current.Select(x => Publish(x).ToObservable()).Concat().Subscribe();
        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping current weather state publisher");
        _subscription?.Dispose();
        return Task.CompletedTask;
    }
}