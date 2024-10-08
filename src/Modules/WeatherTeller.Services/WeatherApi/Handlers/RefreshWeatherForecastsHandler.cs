using MediatR;
using Microsoft.Extensions.Logging;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.Core.WeatherForecasts.Requests;

namespace WeatherTeller.Services.WeatherApi.Handlers;

internal class UserRefreshWeatherForecastsHandler : IRequestHandler<RefreshWeatherForecastCommand>
{
    private readonly IWeatherApi _weatherApi;
    private readonly ILogger<UserRefreshWeatherForecastsHandler> _logger;

    public UserRefreshWeatherForecastsHandler(IWeatherApi weatherApi, ILogger<UserRefreshWeatherForecastsHandler> logger)
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