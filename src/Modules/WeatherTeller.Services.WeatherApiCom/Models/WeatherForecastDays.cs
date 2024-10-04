using System.Text.Json.Serialization;

namespace WeatherTeller.Services.WeatherApiCom.Models;

internal readonly record struct WeatherForecast(
    [property: JsonPropertyName("forecastday")]
    List<WeatherForecastDay> Days
);