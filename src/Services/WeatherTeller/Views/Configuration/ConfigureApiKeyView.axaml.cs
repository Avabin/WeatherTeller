using Avalonia.ReactiveUI;
using ReactiveUI;
using WeatherTeller.ViewModels.Configuration;

namespace WeatherTeller.Views.Configuration;

internal partial class ConfigureApiKeyView : ReactiveUserControl<ConfigureApiKeyViewModel>
{
    public ConfigureApiKeyView()
    {
        InitializeComponent();

        this.WhenActivated(disposables => { });
    }
}