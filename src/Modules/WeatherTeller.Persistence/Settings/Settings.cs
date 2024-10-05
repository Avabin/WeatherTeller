using WeatherTeller.Persistence.Models;

namespace WeatherTeller.Persistence.Settings;

public record Settings : IIdentifiable<string>, IComparable<Settings>, IComparable
{
    public Id<string> Id { get; init; }
    public Location? Location { get; init; }
    public string? ApiKey { get; init; }

    public int CompareTo(Settings? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var locationComparison = Comparer<Location>.Default.Compare(Location, other.Location);
        return locationComparison != 0 ? locationComparison : string.Compare(ApiKey, other.ApiKey, StringComparison.Ordinal);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is Settings other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Settings)}");
    }

    public static bool operator <(Settings? left, Settings? right) => Comparer<Settings>.Default.Compare(left, right) < 0;

    public static bool operator >(Settings? left, Settings? right) => Comparer<Settings>.Default.Compare(left, right) > 0;

    public static bool operator <=(Settings? left, Settings? right) => Comparer<Settings>.Default.Compare(left, right) <= 0;

    public static bool operator >=(Settings? left, Settings? right) => Comparer<Settings>.Default.Compare(left, right) >= 0;
}