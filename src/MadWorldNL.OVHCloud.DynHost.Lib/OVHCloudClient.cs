using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MadWorldNL.OVHCloud.DynHost.Lib.Contracts;
using MadWorldNL.OVHCloud.DynHost.Lib.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MadWorldNL.OVHCloud.DynHost.Lib;

public class OVHCloudClient(IOptions<DynHostSettings> settings, HttpClient httpClient, ILogger<OVHCloudClient> logger) : IOVHCloudClient
{
    private readonly DynHostSettings _settings = settings.Value;
    
    private const string DynHostUrl = "https://dns.eu.ovhapis.com/nic/update?system=dyndns&hostname={0}&myip={1}";
    
    public async Task UpdateIpAddress(string ipAddress)
    {
        foreach (var hostname in _settings.Hostnames)
        {
            logger.LogInformation("Updating hostname {Hostname} to {IpAddress}", hostname, ipAddress);
            await UpdateOneHostName(hostname, ipAddress);
        }
    }

    private async Task UpdateOneHostName(string hostname, string ipAddress)
    {
        var url = string.Format(DynHostUrl, hostname, ipAddress);
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var byteArray = Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        
        try
        {
            var response = await httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            LogResponse(responseBody, ipAddress, hostname);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating host name in OVHCloud");
        }
    }

    private void LogResponse(string responseBody, string ipAddress, string hostName)
    {
        if (responseBody.Contains("good"))
        {
            logger.LogInformation("IP address ({IpAddress}) updated in OVHCloud for {HostName}.", ipAddress, hostName);
        }

        if (responseBody.Contains("nochg"))
        {
            logger.LogInformation("IP address {IpAddress} for {HostName} is already current. No update needed.", ipAddress, hostName);
        }

        if (responseBody.Contains("Client::Unauthorized"))
        {
            logger.LogError("Unauthorized: Authentication failed for {IpAddress} for {HostName}.", ipAddress, hostName);       
        }

        if (responseBody.Contains("Client::BadRequest"))
        {
            var responseJson = JsonSerializer.Deserialize<OVHCloudResponse>(responseBody);
            
            logger.LogError("Bad Request: {Response}", responseJson!.Message);
        }
    }
}