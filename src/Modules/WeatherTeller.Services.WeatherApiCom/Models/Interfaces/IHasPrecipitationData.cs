namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasPrecipitationData
{
    double PrecipitationMm { get; }
    double PrecipitationIn { get; }
}