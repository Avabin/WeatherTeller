using MediatR;
using Microsoft.Extensions.Logging;
using WeatherTeller.Persistence.Core;
using WeatherTeller.Services.Core.Settings.Commands;

namespace WeatherTeller.Services.Settings.Handlers;

internal class UpdateSettingsHandler(ISettingsRepository settingsRepository, ILogger<UpdateSettingsHandler> logger
) : IRequestHandler<UpdateSettingsCommand>
{
    private readonly ISettingsRepository _settingsRepository = settingsRepository;
    private readonly ILogger<UpdateSettingsHandler> _logger = logger;

    public async Task Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Handling request to update settings");
        await _settingsRepository.UpdateSettingsAsync(request.Update);
    }
}