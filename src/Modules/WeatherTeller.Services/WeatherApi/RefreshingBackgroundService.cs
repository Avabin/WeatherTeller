using Microsoft.Extensions.Hosting;
using WeatherTeller.Services.Core.WeatherApi;

namespace WeatherTeller.Services.WeatherApi
{
    public class RefreshingBackgroundService(IWeatherApi weatherApi) : BackgroundService
    {
        private readonly IWeatherApi _weatherApi = weatherApi;
        private const long RefreshInterval = 1000 * 60 * 15; // 15 minutes
        private static readonly TimeSpan RefreshDelay = TimeSpan.FromSeconds(RefreshInterval);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _weatherApi.Refresh();

                await Task.Delay(RefreshDelay, stoppingToken);
            }
        }
    }
}