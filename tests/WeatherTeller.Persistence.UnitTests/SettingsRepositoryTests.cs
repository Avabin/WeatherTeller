using System.Reactive.Linq;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WeatherTeller.Persistence.Core.Notifications;
using WeatherTeller.Persistence.Models;
using WeatherTeller.Persistence.Settings;
using WeatherTeller.Services.Core.Settings;

namespace WeatherTeller.Persistence.UnitTests
{
    [TestFixture, FixtureLifeCycle(LifeCycle.InstancePerTestCase), Parallelizable(ParallelScope.All)]
    public class SettingsRepositoryTests
    {
        private IMediator _mediator;
        private ISettingsDataSource _settingsDataSource;
        private ILogger<SettingsRepository> _logger;
        private SettingsRepository _settingsRepository;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _settingsDataSource = Substitute.For<ISettingsDataSource>();
            _logger = Substitute.For<ILogger<SettingsRepository>>();
            _settingsRepository = new SettingsRepository(_mediator, _settingsDataSource, _logger);
        }

        [Test]
        public async Task GetSettingsAsync_ShouldReturnSettings_WhenSettingsExist()
        {
            // Arrange
            var userName = Environment.UserName;
            var settings = new Persistence.Settings.Settings { Id = userName, ApiKey = "test-api-key" };
            _settingsDataSource.Contains(Arg.Is<Id<string>>(i => i.Value.Equals(userName))).Returns(Observable.Return(true));
            _settingsDataSource.GetById(userName).Returns(Observable.Return(settings));

            // Act
            var result = await _settingsRepository.GetSettingsAsync();

            // Assert
            result.Should().NotBeNull();
            result.ApiKey.Should().Be("test-api-key");
        }

        [Test]
        public async Task GetSettingsAsync_ShouldReturnNull_WhenSettingsDoNotExist()
        {
            // Arrange
            var userName = Environment.UserName;
            _settingsDataSource.GetById(userName).Returns(Observable.Empty<Persistence.Settings.Settings>());

            // Act
            var result = await _settingsRepository.GetSettingsAsync();

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task CreateSettingsAsync_ShouldAddSettingsAndPublishNotification()
        {
            // Arrange
            var settings = new SettingsModel("test-id", new SettingsLocation("", 0.1, 0.1), "test-api-key");

            // Act
            await _settingsRepository.CreateSettingsAsync(settings);

            // Assert
            await _settingsDataSource.Received(1).Add(Arg.Is<Persistence.Settings.Settings>(s => s.Id == "test-id" && s.ApiKey == "test-api-key"));
            await _mediator.Received(1).Publish(Arg.Is<SettingsChangedNotification>(n => n.After == settings), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task UpdateSettingsAsync_ShouldUpdateSettingsAndPublishNotification_WhenSettingsExist()
        {
            // Arrange
            var userName = Environment.UserName;
            var existingSettings = new Persistence.Settings.Settings { Id = userName, ApiKey = "old-api-key" };
            _settingsDataSource.Contains(Arg.Is<Id<string>>(i => i.Value.Equals(userName))).Returns(Observable.Return(true));
            _settingsDataSource.GetById(Arg.Any<Id<string>>()).Returns(Observable.Return(existingSettings));
            Func<SettingsModel, SettingsModel> update = s => s with { ApiKey = "new-api-key" };

            // Act
            await _settingsRepository.UpdateSettingsAsync(update);

            // Assert
            await _settingsDataSource.Received(1).ReplaceOne(Arg.Is<Id<string>>(userName), Arg.Is<Persistence.Settings.Settings>(s => s.ApiKey == "new-api-key"));
            await _mediator.Received(1).Publish(Arg.Is<SettingsChangedNotification>(n => n.After.ApiKey == "new-api-key" && n.Before!.ApiKey == "old-api-key"),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task UpdateSettingsAsync_ShouldNotUpdateSettings_WhenSettingsDoNotExist()
        {
            // Arrange
            var userName = Environment.UserName;
            _settingsDataSource.GetById(Arg.Is<Id<string>>(i => i.Value.Equals(userName))).Returns(Observable.Empty<Persistence.Settings.Settings>());
            Func<SettingsModel, SettingsModel> update = s => s with { ApiKey = "new-api-key" };

            // Act
            await _settingsRepository.UpdateSettingsAsync(update);

            // Assert
            await _settingsDataSource.DidNotReceive().ReplaceOne(Arg.Any<Id<string>>(), Arg.Any<Persistence.Settings.Settings>());
            await _mediator.DidNotReceive().Publish(Arg.Any<SettingsChangedNotification>(), Arg.Any<CancellationToken>());
        }
    }
}