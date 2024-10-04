using System.Text.Json.Serialization;
using WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom.Models;

internal readonly record struct WeatherCondition(
    [property: JsonPropertyName("text")]
    string Text,
    [property: JsonPropertyName("icon")]
    string Icon,
    [property: JsonPropertyName("code")]
    int Code
) : IWeatherCondition
{
    
}