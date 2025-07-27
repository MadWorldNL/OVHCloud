namespace MadWorldNL.OVHCloud.DynHost.Lib.Domain;

public class StorageSettings
{
    public const string Key = nameof(StorageSettings);
    
    public string Path { get; set; } = string.Empty;
}