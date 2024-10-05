using System;
using Avalonia.Data.Converters;
using Avalonia.ReactiveUI;
using ReactiveUI;
using WeatherTeller.ViewModels.WeatherForecast;

namespace WeatherTeller.Views.WeatherForecast;

internal partial class WeatherStateView : ReactiveUserControl<WeatherStateViewModel>
{
    public WeatherStateView()
    {
        InitializeComponent();
        
        this.WhenActivated(disposables =>
        {
            
        });
    }
}

internal class DoubleToIntConverter : FuncValueConverter<double, int>
{
    public DoubleToIntConverter() : base(d => System.Convert.ToInt32(Math.Round(d)))
    {
    }
}