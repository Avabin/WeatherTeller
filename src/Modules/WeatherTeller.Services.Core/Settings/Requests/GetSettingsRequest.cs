using MediatR;

namespace WeatherTeller.Services.Core.Settings.Requests;

public record GetSettingsRequest : IRequest<SettingsModel>
{
    
}