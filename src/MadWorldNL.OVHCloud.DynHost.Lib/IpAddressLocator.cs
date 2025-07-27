using System.Net;
using System.Net.Sockets;
using MadWorldNL.OVHCloud.DynHost.Lib.Contracts;
using MadWorldNL.OVHCloud.DynHost.Lib.Domain;
using Microsoft.Extensions.Logging;

namespace MadWorldNL.OVHCloud.DynHost.Lib;

public class IpAddressLocator(HttpClient httpClient, ILogger<IpAddressLocator> logger) : IIpdAddressLocator
{
    private const string IpAddressLocatorUrl = "https://api.ipify.org";
    
    public async Task<string> GetIpAddress()
    {
        try
        {
            logger.LogInformation("Trying to get ip address from {Url}", IpAddressLocatorUrl);
            return await httpClient.GetStringAsync(IpAddressLocatorUrl);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to get ip address from {Url}", IpAddressLocatorUrl);
            throw new IpAddressLocationFailedException(exception);
        }
    }
}