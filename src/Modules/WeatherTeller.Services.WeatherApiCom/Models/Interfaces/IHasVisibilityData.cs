namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasVisibilityData
{
    double VisibilityKm { get; }
    double VisibilityMiles { get; }
}