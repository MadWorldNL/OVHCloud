using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using MadWorldNL.OVHCloud.DynHost.Lib.Contracts;
using MadWorldNL.OVHCloud.DynHost.Lib.Domain;
using MadWorldNL.OVHCloud.DynHost.Lib.External;
using Microsoft.Extensions.Logging;

namespace MadWorldNL.OVHCloud.DynHost.Lib;

public class IpAddressLocator(HttpClient httpClient, ILogger<IpAddressLocator> logger) : IIpdAddressLocator
{
    private const string IpAddressLocatorUrl = "https://tools-api.mad-world.eu/IpAddress/WhatIsMyIp";
    
    public async Task<string> GetIpAddress()
    {
        try
        {
            logger.LogInformation("Trying to get ip address from {Url}", IpAddressLocatorUrl);
            var response = await httpClient.GetFromJsonAsync<IpAddressResponse>(IpAddressLocatorUrl);
            return response?.IpAddress ?? string.Empty;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to get ip address from {Url}", IpAddressLocatorUrl);
            throw new IpAddressLocationFailedException(exception);
        }
    }
}