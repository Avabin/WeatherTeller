using System.Text.Json.Serialization;
using WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom.Models;

internal readonly record struct DayForecast(
    [property: JsonPropertyName("maxtemp_c")]
    double MaxTempCelsius,
    [property: JsonPropertyName("maxtemp_f")]
    double MaxTempFahrenheit,
    [property: JsonPropertyName("mintemp_c")]
    double MinTempCelsius,
    [property: JsonPropertyName("mintemp_f")]
    double MinTempFahrenheit,
    [property: JsonPropertyName("avgtemp_c")]
    double AvgTempCelsius,
    [property: JsonPropertyName("avgtemp_f")]
    double AvgTempFahrenheit,
    [property: JsonPropertyName("maxwind_mph")]
    double MaxWindMph,
    [property: JsonPropertyName("maxwind_kph")]
    double MaxWindKph,
    [property: JsonPropertyName("totalprecip_mm")]
    double TotalPrecipitationMm,
    [property: JsonPropertyName("totalprecip_in")]
    double TotalPrecipitationIn,
    [property: JsonPropertyName("avgvis_km")]
    double AvgVisibilityKm,
    [property: JsonPropertyName("avgvis_miles")]
    double AvgVisibilityMiles,
    [property: JsonPropertyName("avghumidity")]
    double AvgHumidity,
    [property: JsonPropertyName("daily_will_it_rain")]
    int WillItRain,
    [property: JsonPropertyName("daily_chance_of_rain")]
    int ChanceOfRain,
    [property: JsonPropertyName("daily_will_it_snow")]
    int WillItSnow,
    [property: JsonPropertyName("daily_chance_of_snow")]
    int ChanceOfSnow,
    [property: JsonPropertyName("condition")]
    WeatherCondition Condition,
    [property: JsonPropertyName("uv")] double UvIndex
) : IHasUvData, IHasWillItRainData, IHasWillItSnowData;