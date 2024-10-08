using MediatR;

namespace WeatherTeller.Services.Core.Settings.Commands;

public record UpdateSettingsCommand(Func<SettingsModel, SettingsModel> Update) : IRequest
{
}