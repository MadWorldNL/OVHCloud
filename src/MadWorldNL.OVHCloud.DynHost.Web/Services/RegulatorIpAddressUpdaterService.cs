using MadWorldNL.OVHCloud.DynHost.Lib.Contracts;

namespace MadWorldNL.OVHCloud.DynHost.Web.Services;

public class RegulatorIpAddressUpdaterService(IDynHost dynHost, ILogger<RegulatorIpAddressUpdaterService> logger) : IHostedService
{
    private Timer _timer = null!;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting up regulator ip address updater");
        
        var interval = TimeSpan.FromHours(1);

        _timer = new Timer(
            callback: CheckForIpAddressUpdate,
            state: null,
            dueTime: TimeSpan.Zero,   // First trigger immediately
            period: interval);    
        
        logger.LogInformation("Regulator ip address updater is running.");
        
        return Task.CompletedTask;
    }

    private void CheckForIpAddressUpdate(object? _)
    {
        logger.LogInformation("Triggered: Checking for ip address update");
        
        Task.Run(dynHost.CheckAndUpdateIpAddress);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping regulator ip address updater");
        
        await _timer.DisposeAsync();
        
        logger.LogInformation("Regulator ip address updater is stopped.");
    }
}