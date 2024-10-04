using System.Text.Json.Serialization;

namespace WeatherTeller.Services.WeatherApiCom.Models;

internal readonly record struct CurrentWeatherResponse(
    [property: JsonPropertyName("location")]
    WeatherLocation Location,
    [property: JsonPropertyName("current")]
    WeatherState Current
)
{
}