using MadWorldNL.OVHCloud.DynHost.Lib.Contracts;
using Microsoft.Extensions.Logging;

namespace MadWorldNL.OVHCloud.DynHost.Lib;

public class DynHost(
    IIpdAddressLocator locator,
    IIpAddressStorage storage,
    IOVHCloudClient ovhCloudClient,
    ILogger<DynHost> logger) : IDynHost
{
    public async Task CheckAndUpdateIpAddress()
    {
        var oldIpAddress = storage.GetLastKnownIpAddress();
        var newIpAddress = await locator.GetIpAddress();

        if (string.IsNullOrEmpty(newIpAddress))
        {
            logger.LogError("New ip address not found");
            return;
        }
        
        if (oldIpAddress.Equals(newIpAddress))
        {
            logger.LogInformation("No ip address change found. (Ipaddress: {IpAddress})", oldIpAddress);
            return;
        }
        
        logger.LogInformation("New ip address found. (old ipaddress: {OldIpAddress} & new ipaddress: {NewIpAddress})", oldIpAddress, newIpAddress);
        
        await ovhCloudClient.UpdateIpAddress(newIpAddress);
        storage.SaveLastKnownIpAddress(newIpAddress);
        
        logger.LogInformation("New ip address updated in OVHCloud.");
    }
}