namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasWillItRainData
{
    int WillItRain { get; }
    int ChanceOfRain { get; }
}