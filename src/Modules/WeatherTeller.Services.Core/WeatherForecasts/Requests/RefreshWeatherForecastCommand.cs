using MediatR;

namespace WeatherTeller.Services.Core.WeatherForecast.Requests;

public record RefreshWeatherForecastCommand : IRequest;