using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TorinoRestaurant.Core.Users.Entities;

namespace TorinoRestaurant.Infrastructure.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(e => e.FullName)
                .HasColumnType("nvarchar(128)")
                .IsRequired();
            
            builder.Property(e => e.PhoneNumber)
                .HasColumnType("nvarchar(20)")
                .IsRequired();
            
            builder.Property(e => e.ImageUrl)
                .HasColumnType("nvarchar(256)");
            
            builder.Property(e => e.Password)
                .HasColumnType("nvarchar(128)");
            
            builder.Property(e => e.RefreshToken)
                .HasColumnType("nvarchar(512)");
            
            builder.Property(e => e.RefreshTokenExpiryTime)
                .IsRequired(false);
        }
    }

    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(e => e.Name)
                .HasColumnType("nvarchar(128)")
                .IsRequired();

            builder.Property(e => e.Description)
                .HasColumnType("nvarchar(256)");
        }
    }

    internal class RoleOfUserConfiguration : IEntityTypeConfiguration<RoleOfUser>
    {
        public void Configure(EntityTypeBuilder<RoleOfUser> builder)
        {
            builder.HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId)
                .IsRequired();
            
            builder.HasOne(e => e.User)
                .WithMany(e => e.Roles)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}