using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TorinoRestaurant.Application.AutofactModules;
using TorinoRestaurant.Hosting;
using TorinoRestaurant.Infrastructure.AutofacModules;
using TorinoRestaurant.Migrations.Factories;

namespace TorinoRestaurant.Migrations
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = Worker.CreateBuilder(args)
                            .ConfigureServices((hostContext, services) =>
                            {
                                services.AddHostedService<MigrationJob>();
                            })
                            .ConfigureContainer<ContainerBuilder>((hostContext, container) =>
                            {
                                container.RegisterModule(new InfrastructureModule(DbContextOptionsFactory.Create(hostContext.Configuration), hostContext.Configuration));
                                container.RegisterModule(new ApplicationModule());
                            });

            await hostBuilder.BuildAndRunAsync();
        }

        // EF Core uses this method at design time to access the DbContext
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args);
    }
}