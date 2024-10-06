using System.Runtime.CompilerServices;
using Autofac;
using ReactiveUI;
using WeatherTeller.ViewModels.Configuration;
using WeatherTeller.ViewModels.Core;
using WeatherTeller.ViewModels.Main;
using WeatherTeller.ViewModels.Settings;
using WeatherTeller.ViewModels.WeatherForecast;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;
using Module = Autofac.Module;

[assembly: InternalsVisibleTo("WeatherTeller")]

namespace WeatherTeller.ViewModels;

public class ViewModelsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MainViewModel>().AsSelf().As<ViewModelBase>().As<IScreen>().SingleInstance();
        builder.RegisterType<SettingsViewModel>().AsSelf().As<ViewModelBase>().As<IActivatableViewModel>()
            .As<IRoutableViewModel>().SingleInstance();

        builder.RegisterType<WeatherForecastsViewModel>().AsSelf().As<ViewModelBase>().As<IActivatableViewModel>()
            .SingleInstance();
        builder.RegisterType<WeatherForecastService>().As<IWeatherForecastService>().SingleInstance();
        builder.RegisterType<WeatherStateViewModelFactory>().As<IWeatherStateViewModelFactory>().SingleInstance();
        builder.RegisterType<WeatherForecastDayViewModelFactory>().As<IWeatherForecastDayViewModelFactory>()
            .SingleInstance();

        builder.RegisterType<ConfigurationWizardViewModel>().AsSelf().As<ViewModelBase>()
            .As<IRoutableViewModel>();
        builder.RegisterType<ConfigurationWizardViewModelFactory>().AsSelf();
        builder.RegisterType<ConfigureLocationViewModel>().AsSelf().As<ViewModelBase>().SingleInstance();
        builder.RegisterType<ConfigureApiKeyViewModel>().AsSelf().As<ViewModelBase>().SingleInstance();
    }
}