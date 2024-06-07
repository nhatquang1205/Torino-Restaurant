using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TorinoRestaurant.Hosting;
using TorinoRestaurant.Infrastructure;

namespace TorinoRestaurant.Migrations
{
    public sealed class MigrationJob : Job
    {
        private readonly DataContext _context;

        public MigrationJob(ILogger<MigrationJob> logger,
            DataContext context,
            IHostApplicationLifetime hostApplicationLifetime) : base(logger, hostApplicationLifetime)
        {
            _context = context;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await MigrateDatabaseAsync();
        }

        private async Task MigrateDatabaseAsync()
        {
            Logger.LogInformation("Starting database migration");
            await _context.Database.MigrateAsync();
            Logger.LogInformation("Finished database migration");
        }
    }
}