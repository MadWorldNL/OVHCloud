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

        var fullStoragePath = Path.Combine(settings.Value.Path, _storagePath);
        _filePath = Path.Combine(fullStoragePath, "last-ip.json");

        if (Directory.Exists(fullStoragePath)) return;
        
        _logger.LogInformation("'{Path}' not found, Creating the directory.", fullStoragePath);
        Directory.CreateDirectory(fullStoragePath);
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

        try
        {
            File.WriteAllText(_filePath, json);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to save the last known IP address.");
            throw;
        }

        _logger.LogInformation("Saved last known ip address to {FilePath}", _filePath);
    }

    public void ResetLastKnownIpAddress()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
                _logger.LogInformation("Deleted the last known IP address file at {FilePath}", _filePath);
            }
            else
            {
                _logger.LogInformation("No IP address file found to delete at {FilePath}", _filePath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete IP address file at {FilePath}", _filePath);
            throw;
        }
    }
}