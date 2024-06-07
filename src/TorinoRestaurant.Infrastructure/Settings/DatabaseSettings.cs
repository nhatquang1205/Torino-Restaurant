using Microsoft.Extensions.Configuration;

namespace TorinoRestaurant.Infrastructure.Settings
{
    public sealed class DatabaseSettings
    {
        public static DatabaseSettings Create(IConfiguration configuration)
        {
            var databaseSettings = new DatabaseSettings();
            configuration.GetSection("Database").Bind(databaseSettings);
            return databaseSettings;
        }

        public string? SqlConnectionString { get; set; }
        public string? PostgresConnectionString { get; set; }
    }
}