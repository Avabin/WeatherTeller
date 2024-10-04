using System.Text.Json.Serialization;

namespace WeatherTeller.Services.WeatherApiCom.Models;

internal readonly record struct WeatherForecastDay(
    [property: JsonPropertyName("date")] string Date,
    [property: JsonPropertyName("date_epoch")]
    long DateEpoch,
    [property: JsonPropertyName("day")] DayForecast Day,
    [property: JsonPropertyName("hour")] List<HourlyWeather> Hours
);