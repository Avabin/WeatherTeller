using MediatR;

namespace WeatherTeller.Services.Core.WeatherForecasts.Requests;

public record RefreshWeatherForecastCommand : IRequest;