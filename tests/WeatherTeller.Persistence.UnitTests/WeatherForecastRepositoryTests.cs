using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WeatherTeller.Persistence.Models;
using WeatherTeller.Persistence.WeatherForecasts;
using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.Persistence.UnitTests;

[ExcludeFromCodeCoverage, TestFixture, FixtureLifeCycle(LifeCycle.InstancePerTestCase),
 Parallelizable(ParallelScope.All)]
public class WeatherForecastRepositoryTests
{
    private IWeatherDataSource _weatherForecastDataSource;
    private ILogger<WeatherForecastRepository> _logger;
    private WeatherForecastRepository _weatherForecastRepository;

    [SetUp]
    public void SetUp()
    {
        _weatherForecastDataSource = Substitute.For<IWeatherDataSource>();
        _logger = Substitute.For<ILogger<WeatherForecastRepository>>();
        _weatherForecastRepository = new WeatherForecastRepository(_weatherForecastDataSource, _logger);
    }

    [Test]
    public async Task GetForecastAsync_ShouldReturnForecast_WhenForecastExists()
    {
        // Arrange
        var id = Id<ulong>.New(0);
        var locationBuilder = new WeatherLocationBuilder().WithCity("test-city").WithCountry("test-country").WithLatitude(10).WithLongitude(20);
        var expected = new WeatherForecastBuilder()
            .WithLocation(locationBuilder)
            .WithDays(day => day.WithDate(DateTime.UtcNow).WithState(state =>
                state.WithLocation(locationBuilder)
                    .WithTemperatureC(20).WithCondition("test-condition")
                    .WithPressure(1000)))
            .Build();
        var snapshot = expected.ToSnapshot();
        var expectedLocation = expected.Location;
        var expectedDay = expected.Days[0];
        List<WeatherForecastSnapshot> snapshotList = [snapshot];

        _weatherForecastDataSource.Contains(Arg.Is<Id<ulong>>(i => i.Equals(id))).Returns(Observable.Return(true));
        _weatherForecastDataSource.Where(Arg.Any<Func<WeatherForecastSnapshot, bool>>())
            .Returns(snapshotList.ToAsyncEnumerable());

        // Act
        var results = _weatherForecastRepository.GetWeatherForecastsAsync();
        var result = await results.ToListAsync();
        var actual = result.First();

        // Assert
        actual.Should().NotBeNull();
        actual.Should().NotBeEquivalentTo(WeatherForecast.Empty);
        actual.Location.Should().BeEquivalentTo(expectedLocation);
        actual.Days.First().Should().BeEquivalentTo(expectedDay);
    }

    [Test]
    public async Task GetForecastAsync_ShouldReturnNull_WhenForecastDoesNotExist()
    {
        // Arrange
        var id = Id<ulong>.New(0);
        _weatherForecastDataSource.GetById(id).Returns(Observable.Empty<WeatherForecastSnapshot>());

        // Act
        var result = await _weatherForecastRepository.GetWeatherForecastsAsync().FirstOrDefaultAsync();

        // Assert
        result.Should().BeNull();
    }
}