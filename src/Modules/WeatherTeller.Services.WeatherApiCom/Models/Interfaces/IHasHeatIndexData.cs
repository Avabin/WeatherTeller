namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasHeatIndexData
{
    double HeatIndexCelsius { get; }
    double HeatIndexFahrenheit { get; }
}