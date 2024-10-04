using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.Services.Core.WeatherApi.Notifications;

namespace WeatherTeller.Services.WeatherApi.Publishers
{
    public class TomorrowForecastPublisher : IHostedService
    {
        private readonly IWeatherApi _weatherApi;
        private readonly IMediator _mediator;
        private IDisposable? _subscription;

        public TomorrowForecastPublisher(IWeatherApi weatherApi, IMediator mediator)
        {
            _weatherApi = weatherApi;
            _mediator = mediator;
        }

        private async Task Publish(WeatherForecastDay forecast) => await _mediator.Publish(new TomorrowForecastStateChangedNotification(forecast));

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _subscription = _weatherApi.Tomorrow.Select(x => Publish(x).ToObservable()).Concat().Subscribe();
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken) => _subscription?.Dispose();
    }
}