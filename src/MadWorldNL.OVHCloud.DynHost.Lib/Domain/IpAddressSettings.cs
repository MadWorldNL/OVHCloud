namespace MadWorldNL.OVHCloud.DynHost.Lib.Domain;

public class IpAddressSettings
{
    public const string Key = nameof(IpAddressSettings);
    
    public string Url { get; init; } = string.Empty;
}