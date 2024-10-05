namespace WeatherTeller.Services.Core.WeatherApi.Models;

public readonly record struct WeatherForecastDay(
    DateTimeOffset Date,
    WeatherState State
);