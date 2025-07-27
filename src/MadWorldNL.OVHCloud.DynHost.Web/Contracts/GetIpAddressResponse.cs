namespace MadWorldNL.OVHCloud.DynHost.Web.Contracts;

public class GetIpAddressResponse
{
    public string IpAddressOnline { get; set; } = string.Empty;
    public string LastKnownIpAddressInStorage { get; set; } = string.Empty;
}