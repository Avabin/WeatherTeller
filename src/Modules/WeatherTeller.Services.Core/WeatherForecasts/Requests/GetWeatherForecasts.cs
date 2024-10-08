using MediatR;
using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.Services.Core.WeatherForecasts.Requests;

public class GetWeatherForecasts : IRequest<IEnumerable<WeatherForecast>>
{
    
}