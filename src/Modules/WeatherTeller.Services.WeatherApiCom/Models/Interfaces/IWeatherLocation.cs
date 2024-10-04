namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasWeatherLocation
{
    string Name { get; }
    string Region { get; }
    string Country { get; }
    double Lat { get; }
    double Lon { get; }
    string TzId { get; }
    long LocaltimeEpoch { get; }
    string Localtime { get; }
}