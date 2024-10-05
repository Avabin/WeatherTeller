﻿using System.Reactive.Disposables;
using System.Reactive.Threading.Tasks;
using Commons.ReactiveCommandGenerator.Core;
using MediatR;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.Core.Settings.Commands;
using WeatherTeller.Services.Core.Settings.Requests;
using WeatherTeller.ViewModels.Core;

namespace WeatherTeller.ViewModels.Settings;

internal partial class SettingsViewModel : ViewModelBase, IActivatableViewModel, IRoutableViewModel
{
    private readonly IMediator _mediator;
    
    [Reactive] public string ApiKey { get; set; } = string.Empty;
    [Reactive] public double Latitude { get; set; } = 0;
    [Reactive] public double Longitude { get; set; } = 0;

    [ReactiveCommand]
    private async Task SaveSettings()
    {
        await _mediator.Send(new UpdateSettingsCommand(Update));
        return;

        SettingsModel Update(SettingsModel settings) => settings with { ApiKey = ApiKey, Location = new SettingsLocation("New York", Latitude, Longitude), };
    }

    public SettingsViewModel(IMediator mediator, IScreen hostScreen)
    {
        _mediator = mediator;
        HostScreen = hostScreen;

        this.WhenActivated(disposables =>
        {
            mediator.Send(new GetSettingsRequest())
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