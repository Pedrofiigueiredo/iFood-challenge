using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace tracker
{
  public class ExternalEndpointHealthCheck : IHealthCheck
  {
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      Ping ping = new();
      var reply = await ping.SendPingAsync("http://api.openweathermap.org");

      if (reply.Status != IPStatus.Success)
        return HealthCheckResult.Unhealthy();

      return HealthCheckResult.Healthy();
    }
  }
}