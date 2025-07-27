using MadWorldNL.OVHCloud.DynHost.Lib.Contracts;
using MadWorldNL.OVHCloud.DynHost.Web.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MadWorldNL.OVHCloud.DynHost.Web.Endpoints;

public static class DynHostEndpoints
{
    public static void AddDynHostEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => new DefaultResponse()
        {
            Message = "The api is correctly running!"
        });
        
        app.MapGet("/IpAddress", async (
            IIpdAddressLocator locator,
            IIpAddressStorage storage) => new GetIpAddressResponse()
        {
            IpAddressOnline = await locator.GetIpAddress(),
            LastKnownIpAddressInStorage = storage.GetLastKnownIpAddress()
        });

        app.MapGet("/TriggerIpAddressCheck", async (IDynHost dynHost) =>
        {
            await dynHost.CheckAndUpdateIpAddress();
            return new DefaultResponse()
            {
                Message = ""
            };
        });
    }
}