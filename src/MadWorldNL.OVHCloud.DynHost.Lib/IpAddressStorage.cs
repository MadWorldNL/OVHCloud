using System.Text.Json;
using MadWorldNL.OVHCloud.DynHost.Lib.Contracts;
using MadWorldNL.OVHCloud.DynHost.Lib.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MadWorldNL.OVHCloud.DynHost.Lib;

public class IpAddressStorage : IIpAddressStorage
{
    private readonly ILogger<IpAddressStorage> _logger;
    private readonly string _storagePath = "IpAddressStorage";
    private readonly string _filePath;
    
    public IpAddressStorage(IOptions<StorageSettings> settings, ILogger<IpAddressStorage> logger)
    {
        _logger = logger;
        
        _filePath = Path.Combine(settings.Value.Path, _storagePath, "last-ip.json");

        if (Directory.Exists(_storagePath)) return;
        
        _logger.LogInformation("'{Path}' not found, Creating the directory.", _storagePath);
        Directory.CreateDirectory(_storagePath);
    }
    
    public string GetLastKnownIpAddress()
    {
        if (!File.Exists(_filePath))
        {
            _logger.LogInformation("Last known ip address not found in storage file: {FilePath}.", _filePath);
            return string.Empty;
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<IpAddressCache>(json);
            return data?.IpAddress ?? string.Empty;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to get last known ip address from {FilePath}", _filePath);
            throw new IpAddressStorageCorruptedException(exception);
        }
    }

    public void SaveLastKnownIpAddress(string ipAddress)
    {
        var data = new IpAddressCache { IpAddress = ipAddress };
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(_filePath, json);
        _logger.LogInformation("Saved last known ip address to {FilePath}", _filePath);
    }
}