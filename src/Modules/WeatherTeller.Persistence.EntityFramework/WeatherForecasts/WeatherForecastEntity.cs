using Riok.Mapperly.Abstractions;
using WeatherTeller.Persistence.WeatherForecasts;

namespace WeatherTeller.Persistence.EntityFramework.WeatherForecasts;

internal record WeatherForecastEntity : IHasId<ulong>
{
    public ulong Id { get; set; } = 0;
    public WeatherLocationEntity? Location { get; set; }
    public List<WeatherForecastDayEntity> Days { get; set; } = new();
}

internal record WeatherLocationEntity
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

internal record WeatherForecastDayEntity
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public WeatherStateEntity? State { get; set; } 
}

internal record WeatherStateEntity
{
    public string Condition { get; set; } = string.Empty;
    public double TemperatureC { get; set; }
    public double TemperatureF { get; set; }
    public double Precipitation { get; set; }
    public double Pressure { get; set; }
}

[Mapper]
internal static partial class WeatherForecastEntityMapper
{
    public static partial WeatherForecastEntity ToEntity(this WeatherForecastSnapshot weatherForecast);

    public static partial WeatherForecastSnapshot ToPersistence(this WeatherForecastEntity weatherForecastEntity);
}