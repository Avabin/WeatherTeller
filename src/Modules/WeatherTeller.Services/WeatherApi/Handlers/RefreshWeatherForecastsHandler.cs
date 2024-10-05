using MediatR;
using Microsoft.Extensions.Logging;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.Core.WeatherForecast.Requests;

namespace WeatherTeller.Services.WeatherApi.Handlers;

internal class RefreshWeatherForecastsHandler : IRequestHandler<RefreshWeatherForecastCommand>
{
    private readonly IWeatherApi _weatherApi;
    private readonly ILogger<RefreshWeatherForecastsHandler> _logger;

    public RefreshWeatherForecastsHandler(IWeatherApi weatherApi, ILogger<RefreshWeatherForecastsHandler> logger)
    {
        _weatherApi = weatherApi;
        _logger = logger;
    }
    public async Task Handle(RefreshWeatherForecastCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Refreshing weather forecasts by request");
        await _weatherApi.Refresh();
    }
}