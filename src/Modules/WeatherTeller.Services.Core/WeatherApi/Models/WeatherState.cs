namespace WeatherTeller.Services.Core.WeatherApi.Models;

public readonly record struct WeatherState(
    string Location,
    string Condition,
    double TemperatureC,
    double TemperatureF,
    double Precipitation,
    double Pressure
)
{
}