using Autofac;
using ReactiveUI;
using WeatherTeller.Avalonia.Views;
using WeatherTeller.Avalonia.Views.WeatherForecast;
using WeatherTeller.ViewModels;
using WeatherTeller.ViewModels.WeatherForecast;
using WeatherTeller.ViewModels.WeatherForecast.CurrentWeather;
using MainView = WeatherTeller.Avalonia.Views.Main.MainView;
using MainViewModel = WeatherTeller.ViewModels.Main.MainViewModel;
using MainWindow = WeatherTeller.Avalonia.Views.Main.MainWindow;
using SettingsView = WeatherTeller.Avalonia.Views.Settings.SettingsView;
using SettingsViewModel = WeatherTeller.ViewModels.Settings.SettingsViewModel;

namespace WeatherTeller.Avalonia;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<ViewModelsModule>();
        builder.RegisterType<MainView>().AsSelf().AsImplementedInterfaces().As<IViewFor<MainViewModel>>().SingleInstance();
        builder.RegisterType<MainWindow>().AsSelf().AsImplementedInterfaces().As<IViewFor<MainViewModel>>().SingleInstance();
        builder.RegisterType<SettingsView>().AsSelf().AsImplementedInterfaces().As<IViewFor<SettingsViewModel>>().SingleInstance();
        
        builder.RegisterType<WeatherForecastsView>().AsSelf().AsImplementedInterfaces().As<IViewFor<WeatherForecastsViewModel>>().SingleInstance();
        builder.RegisterType<CurrentWeatherForecastView>().AsSelf().AsImplementedInterfaces().As<IViewFor<CurrentWeatherForecastViewModel>>().SingleInstance();
    }
}