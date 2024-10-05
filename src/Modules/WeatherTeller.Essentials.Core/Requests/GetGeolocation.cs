using MediatR;

namespace WeatherTeller.Essentials.Core.Requests;

public record GetGeolocation : IRequest<Geolocation?>;