using System.Collections.Immutable;
using MediatR;
using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.Services.Core.WeatherApi.Notifications;

public record DaysForecastStateChangedNotification(ImmutableList<WeatherForecastDay> Forecast) : INotification
{
}