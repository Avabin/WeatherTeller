namespace WeatherTeller.Persistence.EntityFramework;

public interface IHasId<T>
{
    T Id { get; set; }
}