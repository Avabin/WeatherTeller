namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasDewPointData
{
    double DewPointCelsius { get; }
    double DewPointFahrenheit { get; }
}