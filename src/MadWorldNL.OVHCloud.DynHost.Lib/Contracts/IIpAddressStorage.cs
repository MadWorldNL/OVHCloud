namespace MadWorldNL.OVHCloud.DynHost.Lib.Contracts;

public interface IIpAddressStorage
{
    public string GetLastKnownIpAddress();
    public void SaveLastKnownIpAddress(string ipAddress);
}