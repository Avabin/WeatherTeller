using System.Reactive.Disposables;
using System.Reactive.Threading.Tasks;
using Commons.ReactiveCommandGenerator.Core;
using MediatR;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WeatherTeller.Persistence.Models;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.ViewModels.Core;
using Unit = System.Reactive.Unit;

namespace WeatherTeller.ViewModels.Settings;

internal partial class SettingsViewModel : ViewModelBase, IActivatableViewModel, IRoutableViewModel
{
    private readonly ISettingsRepository _settingsRepository;
    
    [Reactive] public string ApiKey { get; set; } = string.Empty;
    [Reactive] public double Latitude { get; set; } = 0;
    [Reactive] public double Longitude { get; set; } = 0;

    [ReactiveCommand]
    private async Task SaveSettings()
    {
        await _settingsRepository.UpdateSettingsAsync(settings => settings with
        {
            ApiKey = ApiKey,
            Location = new Location("Custom", Latitude, Longitude),
        });
    }

    public SettingsViewModel(ISettingsRepository settingsRepository, IScreen hostScreen)
    {
        _settingsRepository = settingsRepository;
        HostScreen = hostScreen;

        this.WhenActivated(disposables =>
        {
            _settingsRepository.GetSettingsAsync()
                .ToObservable()
                .Subscribe(settings =>
                {
                    ApiKey = settings.ApiKey ?? string.Empty;
                    Latitude = settings.Location?.Latitude ?? 0;
                    Longitude = settings.Location?.Longitude ?? 0;
                })
                .DisposeWith(disposables);
        });
    }

    public ViewModelActivator Activator { get; } = new();
    public string? UrlPathSegment => $"{nameof(SettingsViewModel)}@{GetHashCode()}";
    public IScreen HostScreen { get; }
}