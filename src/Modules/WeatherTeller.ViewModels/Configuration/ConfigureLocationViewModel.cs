using Commons.ReactiveCommandGenerator.Core;
using MediatR;
using ReactiveUI.Fody.Helpers;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.Core.Settings.Commands;
using WeatherTeller.Services.Core.Settings.Requests;

namespace WeatherTeller.ViewModels.Configuration;

internal partial class ConfigureLocationViewModel : ConfigurationViewModel
{
    private readonly IMediator _mediator;

    public ConfigureLocationViewModel(IMediator mediator) => _mediator = mediator;

    [Reactive] public double Latitude { get; set; }
    [Reactive] public double Longitude { get; set; }

    [ReactiveCommand]
    private async Task Load()
    {
        var settings = await _mediator.Send(new GetSettingsRequest());
        var location = settings?.Location;
        if (location is not null)
        {
            Latitude = location.Latitude;
            Longitude = location.Longitude;
        }
    }

    [ReactiveCommand]
    private async Task Save()
    {
        var settingsLocation = new SettingsLocation("", Latitude, Longitude);
        await _mediator.Send(new UpdateSettingsCommand(s => s with { Location = settingsLocation }));
        IsFinished = true;
    }
}