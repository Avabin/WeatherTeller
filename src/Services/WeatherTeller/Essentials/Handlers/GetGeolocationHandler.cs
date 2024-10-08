using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using WeatherTeller.Essentials.Core.Requests;
using Geolocation = WeatherTeller.Essentials.Core.Requests.Geolocation;

namespace WeatherTeller.Essentials.Handlers;

internal class GetGeolocationHandler : IRequestHandler<GetGeolocation, Geolocation?>
{
    private readonly ILogger<GetGeolocationHandler> _logger;

    public GetGeolocationHandler(ILogger<GetGeolocationHandler> logger) => _logger = logger;

    public async Task<Geolocation?> Handle(GetGeolocation request, CancellationToken cancellationToken) =>
        await GetGeolocation();

    private async Task<Geolocation?> GetGeolocation()
    {
        try
        {
            var location = await Microsoft.Maui.Devices.Sensors.Geolocation.Default.GetLastKnownLocationAsync();
            if (location == null)
                location = await Microsoft.Maui.Devices.Sensors.Geolocation.Default.GetLocationAsync(
                    new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Lowest,
                        RequestFullAccuracy = false
                    });

            return location == null ? null : new Geolocation(location.Latitude, location.Longitude);
        }
        catch (FeatureNotSupportedException e)
        {
            _logger.LogError(e, "Geolocation is not supported on this device");
            return null;
        }
        catch (FeatureNotEnabledException e)
        {
            _logger.LogError(e, "Geolocation is not enabled on this device");
            return null;
        }
        catch (PermissionException e)
        {
            _logger.LogError(e, "Permission denied to access geolocation");
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get geolocation");
            return null;
        }
    }
}