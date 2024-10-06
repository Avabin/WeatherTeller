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
    [Reactive] public string LocationName { get; set; } = string.Empty;
    [Reactive] public double Latitude { get; set; }
    [Reactive] public double Longitude { get; set; }
    
    public ConfigureLocationViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [ReactiveCommand]
    private async Task Load()
    {
        var settings = await _mediator.Send(new GetSettingsRequest());
        var location = settings.Location;
        if (location is not null)
        {
            LocationName = location.Name;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
        }
    }
    
    [ReactiveCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(LocationName) || Latitude == 0 || Longitude == 0)
        {
            return;
        }
        var settingsLocation = new SettingsLocation(LocationName, Latitude, Longitude);
        await _mediator.Send(new UpdateSettingsCommand(s => s with { Location = settingsLocation }));
        IsFinished = true;
    }
}