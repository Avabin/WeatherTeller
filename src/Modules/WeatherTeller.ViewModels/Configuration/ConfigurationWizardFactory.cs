namespace WeatherTeller.ViewModels.Configuration;

internal class ConfigurationWizardViewModelFactory
{
    private readonly ConfigurationWizardViewModel.Factory _factoryMethod;

    public ConfigurationWizardViewModelFactory(ConfigurationWizardViewModel.Factory factoryMethod)
    {
        _factoryMethod = factoryMethod;
    }
    
    public ConfigurationWizardViewModel Create(bool configureLocation = true, bool configureApiKey = true) => 
        _factoryMethod(configureLocation, configureApiKey);
}