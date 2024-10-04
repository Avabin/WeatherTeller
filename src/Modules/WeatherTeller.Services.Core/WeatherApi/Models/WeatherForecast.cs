namespace WeatherTeller.Services.Core.WeatherApi.Models
{
    public readonly record struct WeatherForecast(
        string Location,
        List<WeatherForecastDay> Days
    );
}