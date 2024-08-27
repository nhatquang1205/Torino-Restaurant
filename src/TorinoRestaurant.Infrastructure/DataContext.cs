using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TorinoRestaurant.Application.Commons;
using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Products.Entities;
using TorinoRestaurant.Core.Users.Entities;
using TorinoRestaurant.Infrastructure.Configurations;
using TorinoRestaurant.Infrastructure.Extensions;

namespace TorinoRestaurant.Infrastructure
{
    public sealed class DataContext : DbContext
    {
        private static readonly ILoggerFactory DebugLoggerFactory = new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });
        private IHttpContextAccessor _context;
        private readonly IHostEnvironment? _env;

        public DataContext(DbContextOptions<DataContext> options,
            IHostEnvironment? env) : base(options)
        {
            _env = env;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleOfUser> RoleOfUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }


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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoleConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoleOfUserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly);

            var aggregateTypes = modelBuilder.Model
                                             .GetEntityTypes()
                                             .Select(e => e.ClrType)
                                             .Where(e => !e.IsAbstract && e.IsAssignableTo(typeof(AggregateRoot)));

            foreach (var type in aggregateTypes)
            {
                var aggregateBuild = modelBuilder.Entity(type);
                aggregateBuild.Ignore(nameof(AggregateRoot.DomainEvents));
                aggregateBuild.Property(nameof(AggregateRoot.Id)).ValueGeneratedOnAdd();
            }
        }
  
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }
   
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        private void OnBeforeSaving()
        {
            // Nếu có sự thay đổi dữ liệu
            if (ChangeTracker.HasChanges())
            {
                // Láy các thông tin cơ bản từ hệ thống
                DateTimeOffset now = DateTimeOffset.UtcNow;
                string accountId = GetAccountId();
                // Duyệt qua hết tất cả dối tượng có thay đổi
                foreach (var entry in ChangeTracker.Entries())
                {
                    try
                    {
                        if (entry.Entity is AggregateRoot root)
                        {
                            switch (entry.State)
                            {
                                // Nếu là thêm mới thì cập nhật thông tin thêm mới
                                case EntityState.Added:
                                    {

                                        root.Created = now;
                                        root.CreatedBy = accountId;
                                        break;
                                    }
                                // Nếu là update thì cập nhật các trường liên quan đến update
                                case EntityState.Modified:
                                    {
                                        root.LastModified = now;
                                        root.LastModifiedBy = accountId;
                                        break;
                                    }
                                case EntityState.Deleted:
                                    {
                                        entry.State = EntityState.Modified;
                                        root.DelFlag = true;
                                        root.DeletedOn = now;
                                        root.DeletedBy = accountId;
                                    }
                                    break;
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Lấy user id đang đăng nhập nếu có
        /// <para>Created at: 08/08/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <returns>user id của user đang đăng nhập. Trả về 0 nếu không có thông tin user đăng nhập</returns>
        private string GetAccountId()
        {
            try
            {
                string accountId = "0";
                ClaimsPrincipal user = null;
                if (_context == null)
                {
                    _context = StartupState.Instance.Services.GetService<IHttpContextAccessor>();
                }
                if (_context != null && _context.HttpContext != null)
                {
                    user = _context.HttpContext.User;
                }
                if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
                {
                    var identity = user.Identity as ClaimsIdentity;
                    accountId = identity.Claims.Where(p => p.Type == "UserId").FirstOrDefault()?.Value;
                }
                return accountId;
            }
            catch
            {
                return "0";
            }
        }
    }
}