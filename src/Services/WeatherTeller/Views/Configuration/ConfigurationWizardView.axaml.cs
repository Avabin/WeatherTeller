using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using WeatherTeller.ViewModels.Configuration;

namespace WeatherTeller.Views.Configuration;

internal partial class ConfigurationWizardView : ReactiveUserControl<ConfigurationWizardViewModel>
{
    public ConfigurationWizardView()
    {
        InitializeComponent();
        
        this.WhenActivated(disposables =>
        {
            
        });
    }
}