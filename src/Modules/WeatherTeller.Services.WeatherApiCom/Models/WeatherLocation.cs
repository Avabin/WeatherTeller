using System.Text.Json.Serialization;
using WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom.Models;

internal readonly record struct WeatherLocation(
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("region")]
    string Region,
    [property: JsonPropertyName("country")]
    string Country,
    [property: JsonPropertyName("lat")]
    double Lat,
    [property: JsonPropertyName("lon")]
    double Lon,
    [property: JsonPropertyName("tz_id")]
    string TzId,
    [property: JsonPropertyName("localtime_epoch")]
    long LocaltimeEpoch,
    [property: JsonPropertyName("localtime")]
    string Localtime
) : IHasWeatherLocation
{
    
}