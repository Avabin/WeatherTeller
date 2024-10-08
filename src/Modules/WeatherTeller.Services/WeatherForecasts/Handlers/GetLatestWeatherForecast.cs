using MediatR;
using WeatherTeller.Persistence.Core;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.Services.Core.WeatherForecasts.Requests;

namespace WeatherTeller.Services.WeatherForecasts.Handlers;

public class GetLatestWeatherForecastHandler : IRequestHandler<GetLatestWeatherForecast, WeatherForecast?>
{
    private readonly IWeatherForecastRepository _forecastRepository;

    public GetLatestWeatherForecastHandler(IWeatherForecastRepository forecastRepository) =>
        _forecastRepository = forecastRepository;

    public async Task<WeatherForecast?> Handle(GetLatestWeatherForecast request, CancellationToken cancellationToken)
    {
        var forecast = await _forecastRepository.GetLatestWeatherForecastAsync();

        return forecast;
    }
}