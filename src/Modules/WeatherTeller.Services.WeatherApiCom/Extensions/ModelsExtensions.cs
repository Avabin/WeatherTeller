using Riok.Mapperly.Abstractions;
using WeatherTeller.Services.WeatherApiCom.Models;

namespace WeatherTeller.Services.WeatherApiCom.Extensions;

internal static class ModelsExtensions {
    // From WeatherApiCom.Models.WeatherState to WeatherTeller.Services.Core.WeatherState
    internal static Core.WeatherApi.Models.WeatherState ToCoreModel(this WeatherState weatherState, WeatherLocation location) =>
        new()
        {
            TemperatureC = weatherState.TempC,
            TemperatureF = weatherState.TempF,
            Condition = weatherState.Condition.Text,
            Location = location.ToCoreModel(),
            Precipitation = weatherState.PrecipitationMm,
            Pressure = weatherState.PressureMb
        };

    // From WeatherApiCom.Models.WeatherForecast to WeatherTeller.Services.Core.WeatherForecast
    internal static Core.WeatherApi.Models.WeatherForecast ToCoreModel(this WeatherForecast forecast,
        WeatherLocation location) =>
        new()
        {
            Location = location.ToCoreModel(),
            Days = forecast.Days.Select(day => day.ToCoreModel()).OrderBy(day => day.Date).ToList()
        };

    internal static Core.WeatherApi.Models.WeatherLocation ToCoreModel(this WeatherLocation location) =>
        new(location.Name, location.Country, location.Lat, location.Lon);

    // From WeatherApiCom.Models.WeatherForecastDay to WeatherTeller.Services.Core.WeatherForecastDay
    internal static Core.WeatherApi.Models.WeatherForecastDay ToCoreModel(this WeatherForecastDay forecastDay) =>
        new()
        {
            Date = DateOnly.FromDateTime(DateTimeOffset.Parse(forecastDay.Date).LocalDateTime),
            State = new Core.WeatherApi.Models.WeatherState
            {
                TemperatureC = forecastDay.Day.AvgTempCelsius,
                TemperatureF = forecastDay.Day.AvgTempFahrenheit,
                Condition = forecastDay.Day.Condition.Text,
                Precipitation = forecastDay.Day.TotalPrecipitationMm,
                Pressure = forecastDay.Hours.Select(x => x.PressureMb).Average()
            }
        };
}