using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                   .HasColumnType("varchar(128)")
                   .IsRequired();
            
            builder.Property(e => e.PhoneNumber)
                   .HasColumnType("varchar(20)")
                   .IsRequired();
        }
    }
}