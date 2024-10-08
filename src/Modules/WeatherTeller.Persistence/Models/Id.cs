namespace WeatherTeller.Persistence.Models;

public readonly record struct Id<T>(T Value) : IComparable<Id<T>>
    where T : notnull
{
    public static Id<T> New(T value) => new(value);
    // empty
    public static Id<T> Empty => new(default!);

    public bool Equals(Id<T> other)
    {
        if (Value is null) return other.Value is null;
        return this.Value.Equals(other.Value);
    }

    public bool Equals(Id<T>? other) => other is not null && Equals(other);
    public int CompareTo(Id<T> other) => Value switch
    {
        IComparable<T> comparable => comparable.CompareTo(other.Value),
        _ => throw new InvalidOperationException($"Type {typeof(T)} does not implement IComparable<{typeof(T)}>")
    };
    

    public override int GetHashCode() => Value.GetHashCode();
    public override string? ToString() => Value?.ToString();
    
    // implicit conversion to T
    public static implicit operator T(Id<T> id) => id.Value;
    // implicit conversion from T
    public static implicit operator Id<T>(T value) => Id<T>.New(value);
}