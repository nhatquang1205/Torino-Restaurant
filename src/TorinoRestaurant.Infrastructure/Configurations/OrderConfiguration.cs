using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TorinoRestaurant.Core.Orders.Entities;

namespace TorinoRestaurant.Infrastructure.Configurations
{
    internal class DineInTableConfiguration : IEntityTypeConfiguration<DineInTable>
    {
        public void Configure(EntityTypeBuilder<DineInTable> builder)
        {
            builder.Property(e => e.Name)
                .HasColumnType("nvarchar(256)")
                .IsRequired();
            
            builder.Property(e => e.Sort)
                .IsRequired();

            builder.Property(e => e.IsActive).IsRequired();
        }
    }

    internal class DineInTableHistoryConfiguration : IEntityTypeConfiguration<DineInTableHistory>
    {
        public void Configure(EntityTypeBuilder<DineInTableHistory> builder)
        {
            builder.Property(e => e.TimeStart)
                .HasColumnType("datetimeoffset")
                .IsRequired();
            
            builder.Property(e => e.TimeEnd)
                .HasColumnType("datetimeoffset")
                .IsRequired(false);

            builder.HasOne(e => e.Customer)
                .WithMany(h => h.CustomerTableHistories)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired();

            builder.HasOne(e => e.Staff)
                .WithMany(h => h.StaffTableHistories)
                .HasForeignKey(e => e.StaffId)
                .IsRequired(false);
        }
    }

    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(e => e.Code)
                .HasColumnType("nvarchar(20)")
                .IsRequired();

            builder.HasOne(e => e.TableHistory)
                .WithMany(h => h.Orders)
                .HasForeignKey(e => e.TableHistoryId)
                .IsRequired();

            builder.Property(e => e.TotalPrice)
                .HasColumnType("decimal");

            builder.OwnsOne(e => e.PaymentMethod, tempBuilder =>
            {
                tempBuilder.Property(e => e.Type)
                    .HasColumnName("PaymentMethod")
                    .IsRequired();
            });

            builder.OwnsOne(e => e.OrderStatus, tempBuilder =>
            {
                tempBuilder.Property(e => e.Status)
                    .HasColumnName("Status")
                    .IsRequired();
            });
        }
    }

    internal class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasOne(e => e.Order)
                .WithMany(h => h.OrderDetails)
                .HasForeignKey(e => e.OrderId)
                .IsRequired();
            
            builder.HasOne(e => e.Product)
                .WithMany(h => h.OrderDetails)
                .HasForeignKey(e => e.ProductId)
                .IsRequired();
        }
    }
}