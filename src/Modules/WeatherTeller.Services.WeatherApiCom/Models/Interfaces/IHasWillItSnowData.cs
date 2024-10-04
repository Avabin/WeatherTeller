namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasWillItSnowData
{
    int WillItSnow { get; }
    int ChanceOfSnow { get; }
}