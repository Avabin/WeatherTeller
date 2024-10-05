using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.Services.Core.WeatherApi.Notifications;

namespace WeatherTeller.Services.WeatherApi.Publishers;

internal class CurrentWeatherStatePublisher : IHostedService
{
    private readonly IWeatherApi _weatherApi;
    private readonly IMediator _mediator;
    private IDisposable? _subscription;

    public CurrentWeatherStatePublisher(IWeatherApi weatherApi, IMediator mediator)
    {
        _weatherApi = weatherApi;
        _mediator = mediator;
    }

    private async Task Publish(WeatherState state) => await _mediator.Publish(new CurrentWeatherStateChangedNotification(state));

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _subscription = _weatherApi.Current.Select(x => Publish(x).ToObservable()).Concat().Subscribe();
        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _subscription?.Dispose();
        return Task.CompletedTask;
    }
}