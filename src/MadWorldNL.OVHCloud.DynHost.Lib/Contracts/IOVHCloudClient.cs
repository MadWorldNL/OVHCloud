namespace MadWorldNL.OVHCloud.DynHost.Lib.Contracts;

public interface IOVHCloudClient
{
    Task UpdateIpAddress(string ipAddress);
}