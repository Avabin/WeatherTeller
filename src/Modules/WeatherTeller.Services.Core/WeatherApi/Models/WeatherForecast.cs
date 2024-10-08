using Riok.Mapperly.Abstractions;

namespace WeatherTeller.Services.Core.WeatherApi.Models;

public record WeatherForecast(
    WeatherLocation Location,
    List<WeatherForecastDay> Days
)
{
    public static WeatherForecast Empty => new();
    [MapperConstructor]
    public WeatherForecast() : this(WeatherLocation.Empty, [])
    {
        
    }
}