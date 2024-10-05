using MediatR;
using WeatherTeller.Services.Core.Settings.Commands;

namespace WeatherTeller.Services.Settings.Handlers;

internal class UpdateSettingsHandler(ISettingsRepository settingsRepository) : IRequestHandler<UpdateSettingsCommand>
{
    public async Task Handle(UpdateSettingsCommand request, CancellationToken cancellationToken) => 
        await settingsRepository.UpdateSettingsAsync(request.Update);
}