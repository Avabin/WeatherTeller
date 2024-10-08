namespace WeatherTeller.Services.Core.WeatherApi.Models;

public record WeatherState(
    WeatherLocation Location,
    string Condition,
    double TemperatureC,
    double TemperatureF,
    double Precipitation,
    double Pressure
)
{
    public static WeatherState Empty => new();
    public WeatherState() : this(WeatherLocation.Empty, "", 0, 0, 0, 0)
    {
        
    }
}