using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Data.Models;

namespace ProductManagement.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.ProductId)
                   .UseIdentityColumn(seed: 100000, increment: 1);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Sku)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            builder.Property(p => p.Category)
                  .HasMaxLength(100);

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18, 2)")
                   .IsRequired();

            builder.HasIndex(p => p.Sku)
                   .IsUnique();

            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);

            builder.Property(p => p.CreatedDate)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
