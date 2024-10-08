using Riok.Mapperly.Abstractions;

namespace WeatherTeller.Services.Core.WeatherApi.Models;

public record WeatherLocation(
    string City,
    string Country,
    double Latitude,
    double Longitude
)
{
    public static WeatherLocation Empty => new();

    [MapperConstructor]
    public WeatherLocation() : this("", "", 0, 0)
    {
        
    }
}