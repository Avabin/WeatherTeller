using Commons.ReactiveCommandGenerator.Core;
using MediatR;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WeatherTeller.Services.Core.Settings.Commands;
using WeatherTeller.Services.Core.Settings.Requests;
using WeatherTeller.ViewModels.Core;

namespace WeatherTeller.ViewModels.Configuration;

internal partial class ConfigureApiKeyViewModel : ConfigurationViewModel
{
    private readonly IMediator _mediator;
    [Reactive] public string ApiKey { get; set; } = string.Empty;
    public ConfigureApiKeyViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ReactiveCommand]
    private async Task Load()
    {
        var settings = await _mediator.Send(new GetSettingsRequest());
        ApiKey = settings.ApiKey;
    }
    
    [ReactiveCommand]
    private async Task Save()
    {
        var apiKey = ApiKey;
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return;
        }
        await _mediator.Send(new UpdateSettingsCommand(s => s with { ApiKey = apiKey }));
        IsFinished = true;
    }
}

internal class ConfigurationViewModel : ViewModelBase
{
    [Reactive] public bool IsFinished { get; set; }
}

internal record ConfigurationPageFinished;