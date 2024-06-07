using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TorinoRestaurant.Infrastructure;

namespace TorinoRestaurant.Migrations.Factories
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private readonly IConfiguration _configuration;

        public DataContextFactory()
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json")
                   .AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        public DataContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DataContext CreateDbContext(string[] args)
        {
            return new DataContext(DbContextOptionsFactory.Create(_configuration), null);
        }
    }
}