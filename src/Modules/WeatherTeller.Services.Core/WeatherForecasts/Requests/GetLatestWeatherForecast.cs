using MediatR;
using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.Services.Core.WeatherForecasts.Requests;

public record GetLatestWeatherForecast : IRequest<WeatherForecast?>
{
}