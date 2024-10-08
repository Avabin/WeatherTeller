namespace WeatherTeller.Persistence.Models;

/// <summary>
///     Represents an identifiable item.
/// </summary>
/// <typeparam name="T">The type of the id.</typeparam>
public interface IIdentifiable<T> where T : IComparable<T>
{
    // The id of the item.
    Id<T> Id { get; }
}