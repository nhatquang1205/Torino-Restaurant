using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TorinoRestaurant.Infrastructure;
using TorinoRestaurant.Infrastructure.Settings;

namespace TorinoRestaurant.Migrations.Factories
{
    public static class DbContextOptionsFactory
    {
        public static DbContextOptions<DataContext> Create(IConfiguration configuration)
        {
            var appSettings = DatabaseSettings.Create(configuration);

            return new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(appSettings.SqlConnectionString, b => b.MigrationsAssembly("TorinoRestaurant.Migrations"))
                .Options;
        }
    }
}