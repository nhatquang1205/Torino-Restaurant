using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TorinoRestaurant.Application.Commons;
using TorinoRestaurant.Core.Users.Entities;
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
            await MigrateLocationsAsync();
        }

        private async Task MigrateLocationsAsync()
        {
            var roles = new List<Role>
            {
                CreateRole("Admin", "Admin role"),
                CreateRole("Dev", "Dev"),
                CreateRole("Staff", "Staff"),
                CreateRole("Customer","Customer"),
            };
            if (!await _context.Roles.AnyAsync())
            {
                await _context.Roles.AddRangeAsync(roles);
                await _context.SaveChangesAsync();
            }
            var users = new List<User>
            {
                CreateUser("0932046296", "Trần Nhật Quang", "", Security.GetMD5("Admin@123"), "ADMIN")
            };

            if (!await _context.Users.AnyAsync())
            {
                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();
            }
        }

        private static Role CreateRole(string name, string description)
        {
            return new Role()
            {
                Id = name.ToUpper(),
                Name = name,
                Description = description
            };
        }

        private static User CreateUser(string phoneNumber, string fullName, string imageUrl, string password, string roleId)
        {
            var user = User.Create(phoneNumber, fullName, imageUrl, password);
            var roleOfUser = new RoleOfUser()
            {
                RoleId = roleId,
            };
            user.Roles = [roleOfUser];
            return user;
        }
    }
}