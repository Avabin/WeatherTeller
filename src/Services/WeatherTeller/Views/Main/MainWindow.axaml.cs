using Avalonia.ReactiveUI;
using ReactiveUI;
using MainViewModel = WeatherTeller.ViewModels.Main.MainViewModel;

namespace WeatherTeller.Views.Main;

internal partial class MainWindow : ReactiveWindow<MainViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        
        this.WhenActivated(d => { });
    }
}