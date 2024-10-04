using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Avalonia.Views;
using WeatherTeller.Infrastructure;
using WeatherTeller.ViewModels;
using MainView = WeatherTeller.Avalonia.Views.Main.MainView;
using MainViewModel = WeatherTeller.ViewModels.Main.MainViewModel;
using MainWindow = WeatherTeller.Avalonia.Views.Main.MainWindow;

namespace WeatherTeller.Avalonia;

public partial class App : Application
{
    public static AppHost Host { get; } 
    static App()
    {
        AppHost.AddModule<AppModule>();
        Host = new AppHost();
        Host.Build();
    }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // start the host
        Host.Start();
        // on environment exit
        var vm = Host.Services.GetRequiredService<MainViewModel>();
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                var mainWindow = Host.Services.GetRequiredService<MainWindow>();
                mainWindow.Closing += (sender, args) => _ = Host.StopAsync();
                mainWindow.DataContext = vm;
                desktop.MainWindow = mainWindow;
                break;
            case ISingleViewApplicationLifetime singleViewPlatform:
                var mainView = Host.Services.GetRequiredService<MainView>();
                mainView.DataContext = vm;
                singleViewPlatform.MainView = mainView;
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }
}