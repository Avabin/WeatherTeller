namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasWindData
{
    double WindMph { get; }
    double WindKph { get; }
    int WindDegree { get; }
    string WindDirection { get; }
}