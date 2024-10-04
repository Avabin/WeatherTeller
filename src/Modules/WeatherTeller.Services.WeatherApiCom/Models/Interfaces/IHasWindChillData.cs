namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasWindChillData
{
    double WindChillCelsius { get; }
    double WindChillFahrenheit { get; }
}