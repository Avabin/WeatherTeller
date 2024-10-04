using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.Services.Core.WeatherApi.Notifications;

namespace WeatherTeller.Services.WeatherApi.Publishers
{
    public class DaysForecastPublisher : IHostedService
    {
        private readonly IWeatherApi _weatherApi;
        private readonly IMediator _mediator;
        private IDisposable? _subscription;

        public DaysForecastPublisher(IWeatherApi weatherApi, IMediator mediator)
        {
            _weatherApi = weatherApi;
            _mediator = mediator;
        }

        private async Task Publish(ImmutableList<WeatherForecastDay> forecast) => await _mediator.Publish(new DaysForecastStateChangedNotification(forecast));

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _subscription = _weatherApi.Days.Select(x => Publish(x).ToObservable()).Concat().Subscribe();
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken) => _subscription?.Dispose();
    }
}