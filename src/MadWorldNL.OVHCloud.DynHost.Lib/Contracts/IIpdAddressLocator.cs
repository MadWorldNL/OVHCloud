namespace MadWorldNL.OVHCloud.DynHost.Lib.Contracts;

public interface IIpdAddressLocator
{
    Task<string> GetIpAddress();
}