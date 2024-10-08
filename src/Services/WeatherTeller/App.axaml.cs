using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MsBox.Avalonia;
using WeatherTeller.Infrastructure;
using WeatherTeller.Persistence.EntityFramework;
using MainViewModel = WeatherTeller.ViewModels.Main.MainViewModel;

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
        try
        {
            Host.Build();
            Host.Services.EnsureDbCreated();
            Host.Start();
            // on environment exit
            var vm = Host.Services.GetRequiredService<MainViewModel>();
            vm.CheckSettingsCommand.Execute().Subscribe(unit =>
                Host.Services.GetRequiredService<ILogger<App>>().LogInformation("Settings checked"));
            vm.LoadWeatherForecastsCommand.Execute().Subscribe(unit =>
                Host.Services.GetRequiredService<ILogger<App>>().LogInformation("Weather forecasts loaded"));
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            _ = MessageBoxManager.GetMessageBoxStandard("Error", e.Message).ShowWindowAsync();
        }
    }
}