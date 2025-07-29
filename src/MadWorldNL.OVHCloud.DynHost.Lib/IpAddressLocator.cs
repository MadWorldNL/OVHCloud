using System.Net.Http.Json;
using MadWorldNL.OVHCloud.DynHost.Lib.Contracts;
using MadWorldNL.OVHCloud.DynHost.Lib.Domain;
using MadWorldNL.OVHCloud.DynHost.Lib.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MadWorldNL.OVHCloud.DynHost.Lib;

public class IpAddressLocator(HttpClient httpClient, IOptions<IpAddressSettings> ipAddressSettings, ILogger<IpAddressLocator> logger) : IIpdAddressLocator
{
    public IpAddressSettings IpAddressSettings { get; } = ipAddressSettings.Value;

    public async Task<string> GetIpAddress()
    {
        try
        {
            logger.LogInformation("Trying to get ip address from {Url}", IpAddressSettings.Url);
            var response = await httpClient.GetFromJsonAsync<IpAddressResponse>(IpAddressSettings.Url);
            return response?.IpAddress ?? string.Empty;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to get ip address from {Url}", IpAddressSettings.Url);
            throw new IpAddressLocationFailedException(exception);
        }
    }
}