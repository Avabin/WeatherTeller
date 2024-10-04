using System.Runtime.CompilerServices;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using ReactiveUI;
using WeatherTeller.ViewModels.Core;
using WeatherTeller.ViewModels.WeatherForecast;
using WeatherTeller.ViewModels.WeatherForecast.CurrentWeather;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;
using Module = Autofac.Module;

[assembly: InternalsVisibleTo("WeatherTeller.Avalonia")]
namespace WeatherTeller.ViewModels;

public class ViewModelsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterMediatR(MediatRConfigurationBuilder.Create(typeof(ViewModelsModule).Assembly).Build());
        builder.RegisterType<Main.MainViewModel>().AsSelf().As<ViewModelBase>().As<IScreen>().SingleInstance();
        builder.RegisterType<Settings.SettingsViewModel>().AsSelf().As<ViewModelBase>().As<IActivatableViewModel>().As<IRoutableViewModel>().SingleInstance();
        
        builder.RegisterType<WeatherForecastsViewModel>().AsSelf().As<ViewModelBase>().As<IActivatableViewModel>().SingleInstance();
        builder.RegisterType<WeatherForecastService>().As<IWeatherForecastService>().SingleInstance();
        builder.RegisterType<WeatherStateViewModelFactory>().As<IWeatherStateViewModelFactory>().SingleInstance();
        builder.RegisterType<WeatherForecastDayViewModelFactory>().As<IWeatherForecastDayViewModelFactory>().SingleInstance();
        builder.RegisterType<CurrentWeatherForecastViewModel>().AsSelf().As<ViewModelBase>().As<IActivatableViewModel>().SingleInstance();
    }
}