using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TorinoRestaurant.Hosting
{
    public abstract class Job(ILogger<Job> logger, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    {
        protected readonly ILogger<Job> Logger = logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Logger.LogInformation("Starting Job: {Type}", this.GetType().Name);

                await RunAsync(stoppingToken);

                Logger.LogInformation("Completed Job: {Type}", this.GetType().Name);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error running job - {Ex}", ex.ToString());
                Environment.ExitCode = 1;
                throw;
            }
            _hostApplicationLifetime.StopApplication();
        }

        protected abstract Task RunAsync(CancellationToken cancellationToken);
    }
}