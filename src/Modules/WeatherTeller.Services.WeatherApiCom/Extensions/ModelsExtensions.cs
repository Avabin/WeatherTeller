using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherLocation = WeatherTeller.Services.WeatherApiCom.Models.WeatherLocation;

namespace WeatherTeller.Services.WeatherApiCom.Extensions;

internal static class ModelsExtensions
{
    // From WeatherApiCom.Models.WeatherState to WeatherTeller.Services.Core.WeatherState
    internal static WeatherState ToCoreModel(this Models.WeatherState weatherState, WeatherLocation location) =>
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
    internal static WeatherForecast ToCoreModel(this Models.WeatherForecast forecast,
        WeatherLocation location) =>
        new()
        {
            Location = location.ToCoreModel(),
            Days = forecast.Days.Select(day => day.ToCoreModel(location)).OrderBy(day => day.Date).ToList()
        };

    internal static Core.WeatherApi.Models.WeatherLocation ToCoreModel(this WeatherLocation location) =>
        new(location.Name, location.Country, location.Lat, location.Lon);

    // From WeatherApiCom.Models.WeatherForecastDay to WeatherTeller.Services.Core.WeatherForecastDay
    internal static WeatherForecastDay ToCoreModel(this Models.WeatherForecastDay forecastDay, WeatherLocation location) =>
        new()
        {
            Date = DateOnly.FromDateTime(DateTimeOffset.Parse(forecastDay.Date).LocalDateTime),
            State = new WeatherState
            {
                Location = location.ToCoreModel(),
                TemperatureC = forecastDay.Day.AvgTempCelsius,
                TemperatureF = forecastDay.Day.AvgTempFahrenheit,
                Condition = forecastDay.Day.Condition.Text,
                Precipitation = forecastDay.Day.TotalPrecipitationMm,
                Pressure = forecastDay.Hours.Select(x => x.PressureMb).Average()
            }
        };
}