using Riok.Mapperly.Abstractions;

namespace WeatherTeller.Services.Core.WeatherApi.Models;

public record WeatherForecast(
    WeatherLocation Location,
    List<WeatherForecastDay> Days,
    DateTimeOffset CreatedAt
)
{
    [MapperConstructor]
    public WeatherForecast() : this(WeatherLocation.Empty, [], DateTimeOffset.Now)
    {
    }

    public static WeatherForecast Empty => new();
}