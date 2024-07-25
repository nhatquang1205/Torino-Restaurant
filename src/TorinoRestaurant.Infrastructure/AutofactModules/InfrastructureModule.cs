using Autofac;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Infrastructure.Repositories;
using TorinoRestaurant.Infrastructure.Services;
using TorinoRestaurant.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TorinoRestaurant.Core.Users.Entities;
using TorinoRestaurant.Core.Products.Entities;

namespace TorinoRestaurant.Infrastructure.AutofacModules
{
    public sealed class InfrastructureModule : Module
    {
        private readonly DbContextOptions<DataContext> _options;
        private readonly IConfiguration Configuration;

        public InfrastructureModule(IConfiguration configuration) : this(CreateDbOptions(configuration), configuration)
        {

        }

        public InfrastructureModule(DbContextOptions<DataContext> options, IConfiguration configuration)
        {
            Configuration = configuration;
            _options = options;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(Options.Create(DatabaseSettings.Create(Configuration)));
            builder.RegisterType<DataContext>()
                .AsSelf()
                .InstancePerRequest()
                .InstancePerLifetimeScope()
                .WithParameter(new NamedParameter("options", _options));

            builder.RegisterType<UnitOfWork>()
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .InstancePerLifetimeScope();

            builder.RegisterType(typeof(Repository<User, long>))
                .As(typeof(IRepository<User, long>));

            builder.RegisterType(typeof(Repository<Category, long>))
                .As(typeof(IRepository<Category, long>));

            builder.RegisterType<DateTimeService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<TokenService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        private static DbContextOptions<DataContext> CreateDbOptions(IConfiguration configuration)
        {
            var databaseSettings = DatabaseSettings.Create(configuration);
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseSqlServer(databaseSettings.SqlConnectionString);
            return builder.Options;
        }
    }
}