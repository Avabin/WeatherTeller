using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MsBox.Avalonia;
using WeatherTeller.Infrastructure;
using WeatherTeller.Persistence.EntityFramework;
using WeatherTeller.Views.Main;
using MainViewModel = WeatherTeller.ViewModels.Main.MainViewModel;

namespace WeatherTeller;

using MainView = MainView;
using MainWindow = MainWindow;

public class App : Application
{
    static App()
    {
        AppHost.AddModule<AppModule>();
        Host = new AvaloniaAppHost();
        Host.Build();
    }

    public static AvaloniaAppHost Host { get; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        try
        {
            Host.Build();
            var logger = Host.Services.GetRequiredService<ILogger<App>>();
            Host.Services.EnsureDbCreated();
            Host.Start();
            // on environment exit
            var vm = Host.Services.GetRequiredService<MainViewModel>();
            vm.CheckSettingsCommand.Execute()
                .SelectMany(_ => vm.LoadWeatherForecastsCommand.Execute()
                    .Do(_ => logger.LogInformation("Weather forecasts loaded")))
                .Do(_ => logger.LogInformation("Settings checked"))
                .Subscribe();
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