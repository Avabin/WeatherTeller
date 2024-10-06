using MediatR;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.Core.Settings.Requests;

namespace WeatherTeller.Services.Settings.Handlers;

internal class GetSettingsHandler(ISettingsRepository repository) : IRequestHandler<GetSettingsRequest, SettingsModel?>
{
    public async Task<SettingsModel?> Handle(GetSettingsRequest request, CancellationToken cancellationToken) => 
        await repository.GetSettingsAsync();
}