namespace MadWorldNL.OVHCloud.DynHost.Lib.Domain;

public class IpAddressLocationFailedException(Exception innerException)
    : Exception("Failed to resolve IP address location. See inner exception for details.", innerException);