using MediatR;
using Microsoft.Extensions.Logging;
using WeatherTeller.Persistence.Core;
using WeatherTeller.Services.Core.WeatherForecasts.Requests;

namespace WeatherTeller.Services.WeatherForecasts.Handlers;

internal class PersistWeatherHandler : IRequestHandler<PersistWeatherForecastCommand, ulong>
{
    private readonly ILogger<PersistWeatherHandler> _logger;
    private readonly IWeatherForecastRepository _weatherForecastRepository;

    public PersistWeatherHandler(IWeatherForecastRepository weatherForecastRepository,
        ILogger<PersistWeatherHandler> logger)
    {
        _weatherForecastRepository = weatherForecastRepository;
        _logger = logger;
    }

    public async Task<ulong> Handle(PersistWeatherForecastCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Persisting weather forecast as requested");
        var id = await _weatherForecastRepository.AddWeatherForecastAsync(request.Forecast);

        return id;
    }
}