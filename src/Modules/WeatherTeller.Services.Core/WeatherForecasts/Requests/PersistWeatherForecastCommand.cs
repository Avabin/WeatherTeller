using MediatR;
using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.Services.Core.WeatherForecasts.Requests;

public readonly record struct PersistWeatherForecastCommand(WeatherForecast Forecast) : IRequest<ulong>;