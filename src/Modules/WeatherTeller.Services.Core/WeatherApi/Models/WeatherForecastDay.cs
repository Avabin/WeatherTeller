using System.Runtime.Serialization;
using Riok.Mapperly.Abstractions;

namespace WeatherTeller.Services.Core.WeatherApi.Models;

public record WeatherForecastDay(
    DateOnly Date,
    WeatherState State
)
{
    public static WeatherForecastDay Empty => new();
    [MapperConstructor]
    public WeatherForecastDay() : this(DateOnly.MinValue, WeatherState.Empty)
    {
        
    }
    [IgnoreDataMember] public bool IsToday => Date == DateOnly.FromDateTime(DateTime.UtcNow.Date);
}