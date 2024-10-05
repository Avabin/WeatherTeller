using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Infrastructure;
using WeatherTeller.Persistence.EntityFramework;
using MainView = WeatherTeller.Views.Main.MainView;
using MainViewModel = WeatherTeller.ViewModels.Main.MainViewModel;
using MainWindow = WeatherTeller.Views.Main.MainWindow;

namespace WeatherTeller;

using MainView = Views.Main.MainView;
using MainWindow = Views.Main.MainWindow;

public partial class App : Application
{
    public static AvaloniaAppHost Host { get; } 
    static App()
    {
        AppHost.AddModule<AppModule>();
        Host = new AvaloniaAppHost();
        Host.Build();
    }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Host.Build();
        Host.Services.EnsureDbCreated();
        Host.Start();
        // on environment exit
        var vm = Host.Services.GetRequiredService<MainViewModel>();
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                var mainWindow = Host.Services.GetRequiredService<MainWindow>();
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