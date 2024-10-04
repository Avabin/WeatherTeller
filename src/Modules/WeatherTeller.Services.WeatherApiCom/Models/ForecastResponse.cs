using System.Text.Json.Serialization;

namespace WeatherTeller.Services.WeatherApiCom.Models;

internal readonly record struct ForecastResponse(
    [property: JsonPropertyName("location")]
    WeatherLocation Location,
    [property: JsonPropertyName("forecast")]
    WeatherForecast WeatherForecast
);