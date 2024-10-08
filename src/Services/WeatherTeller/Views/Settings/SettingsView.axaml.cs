﻿using Avalonia.ReactiveUI;
using ReactiveUI;
using SettingsViewModel = WeatherTeller.ViewModels.Settings.SettingsViewModel;

namespace WeatherTeller.Views.Settings;

internal partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();

        this.WhenActivated(disposables => { });
    }
}