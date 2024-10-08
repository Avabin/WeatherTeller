using MediatR;
using Microsoft.Extensions.Logging;
using WeatherTeller.Persistence.Core;
using WeatherTeller.Services.Core.Settings;
using WeatherTeller.Services.Core.Settings.Requests;

namespace WeatherTeller.Services.Settings.Handlers;

internal class GetSettingsHandler(ISettingsRepository repository, ILogger<GetSettingsHandler> logger
) : IRequestHandler<GetSettingsRequest, SettingsModel?>
{
    private readonly ILogger<GetSettingsHandler> _logger = logger;

    public async Task<SettingsModel?> Handle(GetSettingsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Handling request to get settings");
        return await repository.GetSettingsAsync();
    }
}