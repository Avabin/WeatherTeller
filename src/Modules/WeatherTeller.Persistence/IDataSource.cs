using System.Reactive;
using WeatherTeller.Persistence.Models;

namespace WeatherTeller.Persistence;

/// <summary>
/// Represents a data source that provides CRUD operations.
/// </summary>
/// <typeparam name="T">The type of the items in the data source.</typeparam>
/// <typeparam name="TId">The type of the id of the items in the data source.</typeparam>
public interface IDataSource<T, TId> where T : IIdentifiable<TId> where TId : IComparable<TId>
{
    /// <summary>
    /// Returns an async enumerable of items that satisfy the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter the items.</param>
    /// <returns>An async enumerable of items that satisfy the predicate.</returns>
    IAsyncEnumerable<T> Where(Func<T, bool>? predicate = null);

    /// <summary>
    /// Adds an item to the data source.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    IObservable<Unit> Add(T item);

    /// <summary>
    /// Adds a range of items to the data source.
    /// </summary>
    /// <param name="items">The items to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    IObservable<Unit> AddRange(IEnumerable<T> items);

    /// <summary>
    /// Updates an item in the data source.
    /// </summary>
    /// <param name="id">The id of the item to update.</param>
    /// <param name="update">The action to update the item.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    IObservable<Unit> UpdateOne(Id<TId> id, Func<T, T> update);
    
    /// <summary>
    /// Replaces an item in the data source.
    /// </summary>
    /// <param name="id">Id of the item to replace.</param>
    /// <param name="item">The item to replace.</param>
    /// <returns></returns>
    IObservable<Unit> ReplaceOne(Id<TId> id, T item);

    /// <summary>
    /// Updates many items in the data source.
    /// </summary>
    /// <param name="ids">The ids of the items to update.</param>
    /// <param name="update">The action to update the items.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    IObservable<Unit> UpdateMany(IEnumerable<Id<TId>> ids, Func<T, T> update);

    /// <summary>
    /// Removes an item from the data source.
    /// </summary>
    /// <param name="id">The id of the item to remove.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    IObservable<Unit> RemoveOne(Id<TId> id);

    /// <summary>
    /// Removes all items from the data source.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    IObservable<Unit> RemoveAll();

    /// <summary>
    /// Checks if the data source contains an item with the specified id.
    /// </summary>
    /// <param name="id">The id of the item to check.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    IObservable<bool> Contains(Id<TId> id);

    /// <summary>
    /// Gets an item by its id.
    /// </summary>
    /// <param name="id">The id of the item to get.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    IObservable<T?> GetById(Id<TId> id);
}