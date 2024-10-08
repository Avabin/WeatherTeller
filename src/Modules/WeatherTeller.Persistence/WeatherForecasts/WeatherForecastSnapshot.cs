using Riok.Mapperly.Abstractions;
using WeatherTeller.Persistence.Models;

namespace WeatherTeller.Persistence.WeatherForecasts;

public record WeatherForecastSnapshot(
    Id<ulong> Id,
    WeatherLocationSnapshot Location,
    List<WeatherForecastDaySnapshot> Days,
    DateTimeOffset CreatedAt) : IIdentifiable<ulong>
{
    [MapperConstructor]
    public WeatherForecastSnapshot() : this(Id<ulong>.Empty, WeatherLocationSnapshot.Empty, [], DateTimeOffset.Now)
    {
    }
}

public record WeatherForecastDaySnapshot(DateOnly Date, WeatherStateSnapshot State)
{
    [MapperConstructor]
    public WeatherForecastDaySnapshot() : this(DateOnly.MinValue, WeatherStateSnapshot.Empty)
    {
    }
}

public record WeatherStateSnapshot(
    WeatherLocationSnapshot Location,
    string Condition,
    double TemperatureC,
    double TemperatureF,
    double Precipitation,
    double Pressure
)
{
    [MapperConstructor]
    public WeatherStateSnapshot() : this(WeatherLocationSnapshot.Empty, "", 0, 0, 0, 0)
    {
    }

    public static WeatherStateSnapshot Empty => new();
}

public record WeatherLocationSnapshot(string City, string Country, double Latitude, double Longitude)
{
    [MapperConstructor]
    public WeatherLocationSnapshot() : this("", "", 0, 0)
    {
    }

    public static WeatherLocationSnapshot Empty => new();
}