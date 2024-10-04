using System.Text.Json.Serialization;
using WeatherTeller.Services.WeatherApiCom.Models;

namespace WeatherTeller.Services.WeatherApiCom.Extensions;

public static class ModelsExtensions {
    // From WeatherApiCom.Models.WeatherState to WeatherTeller.Services.Core.WeatherState
    internal static Core.WeatherApi.Models.WeatherState ToCoreModel(this WeatherState weatherState, WeatherLocation location) =>
        new()
        {
            TemperatureC = weatherState.TempC,
            TemperatureF = weatherState.TempF,
            Condition = weatherState.Condition.Text,
            Location = location.Name,
            Precipitation = weatherState.PrecipitationMm
        };

    // From WeatherApiCom.Models.WeatherForecast to WeatherTeller.Services.Core.WeatherForecast
    internal static Core.WeatherApi.Models.WeatherForecast ToCoreModel(this WeatherForecast forecast,
        WeatherLocation location) =>
        new()
        {
            Location = location.Name,
            Days = forecast.Days.Select(day => day.ToCoreModel()).OrderBy(day => day.Date).ToList()
        };

    // From WeatherApiCom.Models.WeatherForecastDay to WeatherTeller.Services.Core.WeatherForecastDay
    internal static Core.WeatherApi.Models.WeatherForecastDay ToCoreModel(this WeatherForecastDay forecastDay) =>
        new()
        {
            Date = DateTimeOffset.Parse(forecastDay.Date),
            State = new Core.WeatherApi.Models.WeatherState
            {
                TemperatureC = forecastDay.Day.AvgTempCelsius,
                TemperatureF = forecastDay.Day.AvgTempFahrenheit,
                Condition = forecastDay.Day.Condition.Text,
                Precipitation = forecastDay.Day.TotalPrecipitationMm
            }
        };
}

public record ErrorResponse(
    [property: JsonPropertyName("error")]
    Error Error
);

public record Error(
    [property: JsonPropertyName("code")]
    int Code,
    [property: JsonPropertyName("message")]
    string Message
);

