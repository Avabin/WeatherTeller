namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasPressureData
{
    double PressureMb { get; }
    double PressureIn { get; }
}