namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasFeelsLikeData
{
    double FeelsLikeCelsius { get; }
    double FeelsLikeFahrenheit { get; }
}