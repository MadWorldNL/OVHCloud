using System.Text.Json.Serialization;

namespace MadWorldNL.OVHCloud.DynHost.Lib.Domain;

public class OVHCloudResponse
{
    [JsonPropertyName("class")]
    public string Class { get; init; } = string.Empty;
    
    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;
}