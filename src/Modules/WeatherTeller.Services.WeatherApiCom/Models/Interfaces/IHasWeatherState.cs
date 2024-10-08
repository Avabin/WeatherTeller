namespace WeatherTeller.Services.WeatherApiCom.Models.Interfaces;

public interface IHasWeatherState :
    IHasPressureData,
    IHasWindData,
    IHasVisibilityData,
    IHasDewPointData,
    IHasHeatIndexData,
    IHasPrecipitationData,
    IHasWindChillData,
    IHasFeelsLikeData,
    IHasUvData,
    IHasCloudData
{
    long LastUpdatedEpoch { get; }
    string LastUpdated { get; }
}