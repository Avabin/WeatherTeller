using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using WeatherTeller.Essentials.Core.Requests;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.Core.Settings.Commands;

namespace WeatherTeller.Services.WeatherApi;

public class CheckGeolocationHostedService : IHostedService
{
    private readonly IMediator _mediator;
    private const int IntervalInMinutes = 30;
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(IntervalInMinutes);
    
    private IDisposable? _subscription;

    public CheckGeolocationHostedService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var observable = Observable.Timer(TimeSpan.Zero, period: Interval);
        
        _subscription = observable
            .SelectMany(_ => _mediator.Send(new GetGeolocation(), cancellationToken).ToObservable())
            .Where(geolocation => geolocation != null)
            .Select(geolocation => geolocation!)
            .Select(geolocation => new UpdateSettingsCommand(s => 
                s with { Location = new SettingsLocation("Custom",geolocation.Latitude, geolocation.Longitude) }))
            .SelectMany(command => _mediator.Send(command, cancellationToken).ToObservable())
            .Subscribe();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _subscription?.Dispose();
        return Task.CompletedTask;
    }
}