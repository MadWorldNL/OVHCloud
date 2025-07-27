# OVHCloud
## DynHost
This is a lightweight service that automatically updates your public IP address with [OVHCloud DynHost](https://help.ovhcloud.com/csm/en-ie-dns-dynhost?id=kb_article_view&sysparm_article=KB0051641) every hour.
It's useful for maintaining up-to-date DNS records when using a dynamic IP address (e.g., for home servers or remote access).
The service runs via Docker and provides simple HTTP endpoints for manual control and monitoring.

### Prerequisite
* Docker Desktop

### Installation
1. Download and open ```docker-compose-dynhost.yml```.
2. Update the following environment variables with your OVHCloud DynHost credentials and hostnames:
```bash
- "DynHostSettings__HostNames__0=Test"
- "DynHostSettings__Username=Username"
- "DynHostSettings__Password=Password"
```

3. From the root of this repository, run the app using Docker Compose:
```bash
docker compose -f docker-compose-dynhost.yml
```

### Usage
Once running, the service will automatically push your current IP address to OVHCloud every hour.

You can also interact with the following endpoints:
* **View IP Address Status**\
  Returns the current public IP and the last saved IP sent to OVHCloud:\
  http://localhost:6500/IpAddress

* **Trigger Immediate IP Check**\
  Forces an immediate check and update of your IP address, skipping the hourly schedule:\
  http://localhost:6500/TriggerIpAddressCheck

* **Reset Cache and Force Update**\
  Clears the cached IP address and ensures your current IP is sent during the next check:\
  http://localhost:6500/ResetIpAddressCheck
