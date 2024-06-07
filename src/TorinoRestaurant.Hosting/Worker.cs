using Microsoft.Extensions.Hosting;

namespace TorinoRestaurant.Hosting
{
    public static class Worker
    {
        public static IHostBuilder CreateBuilder(string[] args)
        {
            return Application.CreateBuilder(args)
                .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production")
                .UseConsoleLifetime();
        }
    }
}