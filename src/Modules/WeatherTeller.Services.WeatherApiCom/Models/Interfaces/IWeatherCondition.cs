namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IWeatherCondition
{
    string Text { get; }
    string Icon { get; }
    int Code { get; }
}