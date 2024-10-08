﻿
namespace WeatherTeller.Services.Core.Settings;

public record SettingsLocation(string City, double Latitude, double Longitude)
{
}

public record SettingsModel(string Id, SettingsLocation Location, string ApiKey)
{
    public static SettingsModel Default => new("", new("", 0, 0), "");
    public static SettingsModel WithDefaultUser(string location, string apiKey, double latitude, double longitude) => new(Environment.UserName, new(location, latitude, longitude), apiKey);
}