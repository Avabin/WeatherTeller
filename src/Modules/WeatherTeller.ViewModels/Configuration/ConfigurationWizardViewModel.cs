using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Commons.ReactiveCommandGenerator.Core;
using DynamicData.Binding;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WeatherTeller.ViewModels.Core;

namespace WeatherTeller.ViewModels.Configuration;

internal partial class ConfigurationWizardViewModel : ViewModelBase, IRoutableViewModel, IActivatableViewModel
{
    public delegate ConfigurationWizardViewModel Factory(bool configureLocation, bool configureApiKey);

    private readonly ILogger<ConfigurationWizardViewModel> _logger;

    public ConfigurationWizardViewModel(IScreen hostScreen, ConfigureLocationViewModel configureLocationViewModel,
        ConfigureApiKeyViewModel configureApiKeyViewModel,
        ILogger<ConfigurationWizardViewModel> logger,
        bool configureLocation, bool configureApiKey)
    {
        _logger = logger;
        var configureLocation1 = configureLocation;
        var configureApiKey1 = configureApiKey;
        HostScreen = hostScreen;

        Pages = new List<ConfigurationViewModel>();

        if (configureLocation1) Pages.Add(configureLocationViewModel);

        if (configureApiKey1) Pages.Add(configureApiKeyViewModel);

        var hasPages =
            (configureApiKey1 && configureLocation1) || configureApiKey1 || configureLocation1; // at least one page
        if (!hasPages) throw new ArgumentException("At least one page must be configured");

        this.WhenActivated(d =>
        {
            var hasMorePages = Pages.Count > 1;
            var allPagesObservable = hasMorePages
                ? Pages.Select(p => p.WhenValueChanged(p => p.IsFinished)).ToList()
                : [Pages.First().WhenValueChanged(p => p.IsFinished)];

            allPagesObservable.Merge()
                .Where(x => x)
                .Do(_ => _logger.LogInformation("Page finished"))
                .Select(_ => Unit.Default)
                .InvokeCommand(CheckCommand)
                .DisposeWith(d);
        });
    }

    [Reactive] public int CurrentPageIndex { get; set; }

    public List<ConfigurationViewModel> Pages { get; }

    public ViewModelActivator Activator { get; } = new();

    public string? UrlPathSegment { get; } = "configuration";
    public IScreen HostScreen { get; }

    [ReactiveCommand]
    private async Task Check()
    {
        var allPagesState = Pages.All(p => p.IsFinished);
        if (allPagesState)
            await HostScreen.Router.NavigateBack.Execute();
        else
            // go to next page
            CurrentPageIndex++;
    }
}