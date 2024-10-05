using Autofac;
using ReactiveUI;
using WeatherTeller.ViewModels;
using WeatherTeller.ViewModels.WeatherForecast;
using WeatherTeller.ViewModels.WeatherForecast.CurrentWeather;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;
using WeatherTeller.Views.WeatherForecast;
using MainView = WeatherTeller.Views.Main.MainView;
using MainViewModel = WeatherTeller.ViewModels.Main.MainViewModel;
using MainWindow = WeatherTeller.Views.Main.MainWindow;
using SettingsView = WeatherTeller.Views.Settings.SettingsView;
using SettingsViewModel = WeatherTeller.ViewModels.Settings.SettingsViewModel;

namespace WeatherTeller;

using MainView = Views.Main.MainView;
using MainWindow = Views.Main.MainWindow;
using SettingsView = Views.Settings.SettingsView;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<ViewModelsModule>();
        builder.RegisterType<MainView>().AsSelf().AsImplementedInterfaces().As<IViewFor<MainViewModel>>();
        builder.RegisterType<MainWindow>().AsSelf().AsImplementedInterfaces().As<IViewFor<MainViewModel>>();
        builder.RegisterType<SettingsView>().AsSelf().AsImplementedInterfaces().As<IViewFor<SettingsViewModel>>();
        
        builder.RegisterType<WeatherForecastsView>().AsSelf().AsImplementedInterfaces().As<IViewFor<WeatherForecastsViewModel>>();
        builder.RegisterType<CurrentWeatherForecastView>().AsSelf().AsImplementedInterfaces().As<IViewFor<CurrentWeatherForecastViewModel>>();
        builder.RegisterType<WeatherForecastDayView>().AsSelf().AsImplementedInterfaces().As<IViewFor<WeatherForecastDayViewModel>>();
    }
}