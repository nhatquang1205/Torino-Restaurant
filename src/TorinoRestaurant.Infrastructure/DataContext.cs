using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Users.Entities;
using TorinoRestaurant.Infrastructure.Configurations;
using TorinoRestaurant.Infrastructure.Extensions;

namespace TorinoRestaurant.Infrastructure
{
    public sealed class DataContext : DbContext
    {
        private static readonly ILoggerFactory DebugLoggerFactory = new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });
        private readonly IHostEnvironment? _env;

        public DataContext(DbContextOptions<DataContext> options,
            IHostEnvironment? env) : base(options)
        {
            _env = env;
        }

        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_env != null && _env.IsDevelopment())
            {
                // used to print activity when debugging
                optionsBuilder.UseLoggerFactory(DebugLoggerFactory);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            var aggregateTypes = modelBuilder.Model
                                             .GetEntityTypes()
                                             .Select(e => e.ClrType)
                                             .Where(e => !e.IsAbstract && e.IsAssignableTo(typeof(AggregateRoot)));

            foreach (var type in aggregateTypes)
            {
                var aggregateBuild = modelBuilder.Entity(type);
                aggregateBuild.Ignore(nameof(AggregateRoot.DomainEvents));
                aggregateBuild.Property(nameof(AggregateRoot.Id)).ValueGeneratedNever();
            }
        }
    }
}