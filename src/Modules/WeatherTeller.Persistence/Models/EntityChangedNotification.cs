using System.Reactive.Linq;
using MediatR;

namespace WeatherTeller.Persistence.Models;

public record EntityChangedNotification<T, TId>(DateTimeOffset Timestamp, T? Before, T After)
    : INotification where T : IIdentifiable<TId>, IComparable<T> where TId : IComparable<TId>
{
}

public static class EntityObservableExtensions
{
    // Before method that filters the notifications before a certain time
    public static IObservable<TNotification> Before<TNotification, T, TId>(this IObservable<TNotification> source,
        DateTimeOffset timestamp)
        where TNotification : EntityChangedNotification<T, TId>
        where T : IIdentifiable<TId>, IComparable<T>
        where TId : IComparable<TId> =>
        source.Where(x => x.Timestamp < timestamp);

    // After method that filters the notifications after a certain time
    public static IObservable<TNotification> After<TNotification, T, TId>(
        this IObservable<TNotification> source, DateTimeOffset timestamp)
        where TNotification : EntityChangedNotification<T, TId>
        where T : IIdentifiable<TId>, IComparable<T>
        where TId : IComparable<TId> =>
        source.Where(x => x.Timestamp > timestamp);

    // WhereFieldChanged method that accepts a field and filters out the notifications that have this field unchanged
    public static IObservable<TNotification> WhereFieldChanged<TNotification, T, TId, TField>(
        this IObservable<TNotification> source, Func<T, TField?> fieldSelector)
        where TNotification : EntityChangedNotification<T, TId>
        where T : IIdentifiable<TId>, IComparable<T>
        where TId : IComparable<TId> =>
        source.Where(x =>
        {
            if (x.Before == null)
                return false;
            var beforeField = fieldSelector(x.Before);
            var afterField = fieldSelector(x.After);
            return !beforeField?.Equals(afterField) ?? false;
        });

    // WhereChanged method that filters out the notifications that have entities that have not changed
    public static IObservable<TNotification>
        WhereChanged<TNotification, T, TId>(this IObservable<TNotification> source)
        where TNotification : EntityChangedNotification<T, TId>
        where T : IIdentifiable<TId>, IComparable<T>
        where TId : IComparable<TId> =>
        source.Where(x => !x.Before?.Equals(x.After) ?? false);
    
    // where not null
    public static IObservable<T> WhereNotNull<T>(this IObservable<T?> source) =>
        source.Where(x => x is not null).Select(x => x!);
}