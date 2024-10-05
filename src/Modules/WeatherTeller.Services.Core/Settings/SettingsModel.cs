using WeatherTeller.Persistence.Models;

namespace WeatherTeller.Services.Core.Settings;

public record SettingsLocation(string Name, double Latitude, double Longitude) : IComparable<SettingsLocation>, IComparable
{
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is SettingsLocation other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(SettingsLocation)}");
    }

    public static bool operator <(SettingsLocation left, SettingsLocation right) => left.CompareTo(right) < 0;

    public static bool operator >(SettingsLocation left, SettingsLocation right) => left.CompareTo(right) > 0;

    public static bool operator <=(SettingsLocation left, SettingsLocation right) => left.CompareTo(right) <= 0;

    public static bool operator >=(SettingsLocation left, SettingsLocation right) => left.CompareTo(right) >= 0;

    public int CompareTo(SettingsLocation? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        return other is null ? 1 : string.Compare(Name, other.Name, StringComparison.Ordinal);
    }
}

public record SettingsModel(Id<string> Id, SettingsLocation? Location, string ApiKey) : IIdentifiable<string>, IComparable<SettingsModel>, IComparable
{
    public static SettingsModel Default => new("", new("", 0, 0), "");
    public static SettingsModel WithDefaultUser(string location, string apiKey, double latitude, double longitude) => new(Environment.UserName, new(location, latitude, longitude), apiKey);

    public int CompareTo(SettingsModel? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var locationComparison = Location?.CompareTo(other.Location) ?? -1;
        return locationComparison != 0 ? locationComparison : string.Compare(ApiKey, other.ApiKey, StringComparison.Ordinal);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is SettingsModel other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(SettingsModel)}");
    }

    public static bool operator <(SettingsModel? left, SettingsModel? right) => Comparer<SettingsModel>.Default.Compare(left, right) < 0;

    public static bool operator >(SettingsModel? left, SettingsModel? right) => Comparer<SettingsModel>.Default.Compare(left, right) > 0;

    public static bool operator <=(SettingsModel? left, SettingsModel? right) => Comparer<SettingsModel>.Default.Compare(left, right) <= 0;

    public static bool operator >=(SettingsModel? left, SettingsModel? right) => Comparer<SettingsModel>.Default.Compare(left, right) >= 0;
}