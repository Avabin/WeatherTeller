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
            Location = location.Name,
            Precipitation = weatherState.PrecipitationMm,
            Pressure = weatherState.PressureMb
        };

    // From WeatherApiCom.Models.WeatherForecast to WeatherTeller.Services.Core.WeatherForecast
    internal static Core.WeatherApi.Models.WeatherForecast ToCoreModel(this Models.WeatherForecast forecast,
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
                Precipitation = forecastDay.Day.TotalPrecipitationMm,
                Pressure = forecastDay.Hours.Select(x => x.PressureMb).Average()
            }
        };
}