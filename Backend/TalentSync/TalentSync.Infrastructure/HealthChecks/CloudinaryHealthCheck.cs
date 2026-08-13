using CloudinaryDotNet;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Infrastructure.HealthChecks
{
    public class CloudinaryHealthCheck :IHealthCheck
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryHealthCheck(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cloudinary.PingAsync();

                return HealthCheckResult.Healthy("Cloudinary is reachable.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Cloudinary is unavailable.", ex);
            }
        }
    }
}
