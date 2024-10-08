using System.Text.Json.Serialization;
using WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom.Models;

internal readonly record struct WeatherState(
    [property: JsonPropertyName("last_updated_epoch")]
    long LastUpdatedEpoch,
    [property: JsonPropertyName("last_updated")]
    string LastUpdated,
    [property: JsonPropertyName("temp_c")] double TempC,
    [property: JsonPropertyName("temp_f")] double TempF,
    [property: JsonPropertyName("is_day")] int IsDay,
    [property: JsonPropertyName("condition")]
    WeatherCondition Condition,
    [property: JsonPropertyName("wind_mph")]
    double WindMph,
    [property: JsonPropertyName("wind_kph")]
    double WindKph,
    [property: JsonPropertyName("wind_degree")]
    int WindDegree,
    [property: JsonPropertyName("wind_dir")]
    string WindDirection,
    [property: JsonPropertyName("pressure_mb")]
    double PressureMb,
    [property: JsonPropertyName("pressure_in")]
    double PressureIn,
    [property: JsonPropertyName("precip_mm")]
    double PrecipitationMm,
    [property: JsonPropertyName("precip_in")]
    double PrecipitationIn,
    [property: JsonPropertyName("humidity")]
    int Humidity,
    [property: JsonPropertyName("cloud")] int CloudCover,
    [property: JsonPropertyName("feelslike_c")]
    double FeelsLikeCelsius,
    [property: JsonPropertyName("feelslike_f")]
    double FeelsLikeFahrenheit,
    [property: JsonPropertyName("windchill_c")]
    double WindChillCelsius,
    [property: JsonPropertyName("windchill_f")]
    double WindChillFahrenheit,
    [property: JsonPropertyName("heatindex_c")]
    double HeatIndexCelsius,
    [property: JsonPropertyName("heatindex_f")]
    double HeatIndexFahrenheit,
    [property: JsonPropertyName("dewpoint_c")]
    double DewPointCelsius,
    [property: JsonPropertyName("dewpoint_f")]
    double DewPointFahrenheit,
    [property: JsonPropertyName("vis_km")] double VisibilityKm,
    [property: JsonPropertyName("vis_miles")]
    double VisibilityMiles,
    [property: JsonPropertyName("uv")] double UvIndex,
    [property: JsonPropertyName("gust_mph")]
    double GustMph,
    [property: JsonPropertyName("gust_kph")]
    double GustKph
) : IHasWeatherState
{
}