using Riok.Mapperly.Abstractions;

namespace WeatherTeller.Services.Core.WeatherApi.Models;

public record WeatherLocation(
    string City,
    string Country,
    double Latitude,
    double Longitude
)
{
    [MapperConstructor]
    public WeatherLocation() : this("", "", 0, 0)
    {
    }

    public static WeatherLocation Empty => new();
}