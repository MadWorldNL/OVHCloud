namespace MadWorldNL.OVHCloud.DynHost.Lib.Domain;

public class DynHostSettings
{
    public const string Key = nameof(DynHostSettings);

    public string[] Hostnames { get; set; } = [];
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}