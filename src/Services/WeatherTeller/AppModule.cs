using Autofac;
using ReactiveUI;
using WeatherTeller.ViewModels;
using WeatherTeller.ViewModels.Configuration;
using WeatherTeller.ViewModels.WeatherForecast;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;
using WeatherTeller.Views.Configuration;
using WeatherTeller.Views.Main;
using WeatherTeller.Views.Settings;
using WeatherTeller.Views.WeatherForecast;
using MainViewModel = WeatherTeller.ViewModels.Main.MainViewModel;
using SettingsViewModel = WeatherTeller.ViewModels.Settings.SettingsViewModel;

namespace WeatherTeller;

using MainView = MainView;
using MainWindow = MainWindow;
using SettingsView = SettingsView;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<ViewModelsModule>();
        builder.RegisterType<MainView>().AsSelf().AsImplementedInterfaces().As<IViewFor<MainViewModel>>();
        builder.RegisterType<MainWindow>().AsSelf().AsImplementedInterfaces().As<IViewFor<MainViewModel>>();
        builder.RegisterType<SettingsView>().AsSelf().AsImplementedInterfaces().As<IViewFor<SettingsViewModel>>();

        builder.RegisterType<WeatherForecastsView>().AsSelf().AsImplementedInterfaces()
            .As<IViewFor<WeatherForecastsViewModel>>();
        builder.RegisterType<WeatherForecastDayView>().AsSelf().AsImplementedInterfaces()
            .As<IViewFor<WeatherForecastDayViewModel>>();

        builder.RegisterType<ConfigurationWizardView>().AsSelf().AsImplementedInterfaces()
            .As<IViewFor<ConfigurationWizardViewModel>>();
        builder.RegisterType<ConfigureLocationView>().AsSelf().AsImplementedInterfaces()
            .As<IViewFor<ConfigureLocationViewModel>>();
        builder.RegisterType<ConfigureApiKeyView>().AsSelf().AsImplementedInterfaces()
            .As<IViewFor<ConfigureApiKeyViewModel>>();
    }
}