using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Products;
using Domain.Carts;

namespace Infrasctructure.Database.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Description)
               .HasMaxLength(1024);

        builder.Property(p => p.IsInStock)
               .IsRequired();

        builder.Property(p => p.Quantity)
               .IsRequired();

        builder.Property(p => p.CreatedDate)
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.Rating)
               .IsRequired()
               .HasDefaultValue(0);

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.CategoryId);

        builder.HasMany<CartItem>()
               .WithOne(c => c.Product)
               .HasForeignKey(c => c.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.FavoriteProducts)
       .WithOne(fp => fp.Product)
       .HasForeignKey(fp => fp.ProductId);
    }
}
