using Avalonia.ReactiveUI;
using ReactiveUI;
using MainViewModel = WeatherTeller.ViewModels.Main.MainViewModel;

namespace WeatherTeller.Avalonia.Views.Main;

internal partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
        });
    }
}