using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherTeller.Essentials.Core.Requests;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.Core.Settings.Commands;

namespace WeatherTeller.Services.WeatherApi;

public class CheckGeolocationHostedService : IHostedService
{
    private const int IntervalInMinutes = 30;
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(IntervalInMinutes);
    private readonly ILogger<CheckGeolocationHostedService> _logger;
    private readonly IMediator _mediator;

    private IDisposable? _subscription;

    public CheckGeolocationHostedService(IMediator mediator, ILogger<CheckGeolocationHostedService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace("Starting geo-location check service");
        var observable = Observable.Timer(TimeSpan.Zero, Interval);

        _subscription = observable
            .Do(_ => _logger.LogTrace("Checking geolocation"))
            .SelectMany(_ => _mediator.Send(new GetGeolocation(), cancellationToken).ToObservable())
            .Where(geolocation => geolocation != null)
            .Select(geolocation => geolocation!)
            .Do(geolocation => _logger.LogTrace("Geolocation: {@Geolocation}. Updating settings", geolocation))
            .Select(geolocation => new UpdateSettingsCommand(s =>
                s with { Location = new SettingsLocation("Custom", geolocation.Latitude, geolocation.Longitude) }))
            .SelectMany(command => _mediator.Send(command, cancellationToken).ToObservable())
            .Subscribe();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace("Stopping geo-location check service");
        _subscription?.Dispose();
        return Task.CompletedTask;
    }
}