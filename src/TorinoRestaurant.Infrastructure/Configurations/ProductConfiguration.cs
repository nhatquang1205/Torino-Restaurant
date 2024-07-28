using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TorinoRestaurant.Core.Products.Entities;

namespace TorinoRestaurant.Infrastructure.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(e => e.Name)
                .HasColumnType("nvarchar(256)")
                .IsRequired();
            
            builder.Property(e => e.Description)
                .HasColumnType("nvarchar(512)")
                .IsRequired();
            
            builder.Property(e => e.VietnameseDescription)
                .HasColumnType("nvarchar(512)");
            
            builder.Property(e => e.ImageUrl)
                .HasColumnType("nvarchar(256)");
            
            builder.HasOne(e => e.Category)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(e => e.Name)
                .HasColumnType("nvarchar(128)")
                .IsRequired();
            
            builder.Property(e => e.Description)
                .HasColumnType("nvarchar(512)")
                .IsRequired();
            
            builder.Property(e => e.ImageUrl)
                .HasColumnType("nvarchar(256)");
        }
    }
}