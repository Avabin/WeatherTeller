using MediatR;
using WeatherTeller.Persistence.Core;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.Services.Core.WeatherForecasts.Requests;

namespace WeatherTeller.Services.WeatherForecasts.Handlers;

public class GetWeatherForecastsHandler : IRequestHandler<GetWeatherForecasts, IEnumerable<WeatherForecast>>
{
    private readonly IWeatherForecastRepository _weatherForecastRepository;

    public GetWeatherForecastsHandler(IWeatherForecastRepository weatherForecastRepository) =>
        _weatherForecastRepository = weatherForecastRepository;

    public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherForecasts request,
        CancellationToken cancellationToken)
    {
        var forecasts = await _weatherForecastRepository.GetWeatherForecastsAsync()
            .Where(forecast =>
                forecast.CreatedAt.Date >= request.NotBefore.ToDateTime(TimeOnly.MinValue, DateTimeKind.Local))
            .ToListAsync(cancellationToken);

        return forecasts;
    }
}