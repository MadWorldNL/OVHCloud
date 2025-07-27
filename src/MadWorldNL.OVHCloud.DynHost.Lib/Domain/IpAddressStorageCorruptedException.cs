namespace MadWorldNL.OVHCloud.DynHost.Lib.Domain;

public class IpAddressStorageCorruptedException(Exception innerException) : Exception("IP address storage is corrupted or unreadable. See inner exception for details.", innerException)
{
    
}