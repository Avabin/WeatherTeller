using WeatherTeller.Persistence.Models;

namespace WeatherTeller.Persistence.WeatherForecasts;

public record WeatherForecastSnapshot(Id<ulong> Id, string Location, List<WeatherForecastDaySnapshot> Days) : IIdentifiable<ulong>;

public record WeatherForecastDaySnapshot(Id<ulong> Id, DateOnly Date, WeatherStateSnapshot State) : IIdentifiable<ulong>;

public record WeatherStateSnapshot(
    string Condition,
    double TemperatureC,
    double TemperatureF,
    double Precipitation,
    double Pressure
);
