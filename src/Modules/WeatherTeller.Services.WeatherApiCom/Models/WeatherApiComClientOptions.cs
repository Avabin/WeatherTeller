namespace WeatherTeller.Services.WeatherApiCom.Models;

public record WeatherApiComClientOptions
{
    public const string SectionName = "WeatherApiCom";
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.weatherapi.com/v1/";

    public string CurrentWeatherEndpoint { get; set; } = "current.json";
    public string ForecastEndpoint { get; set; } = "forecast.json";
    public int DefaultDays { get; set; } = 31;
}