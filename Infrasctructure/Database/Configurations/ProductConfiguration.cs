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

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
               .IsRequired()  
               .HasMaxLength(200);

        builder.Property(product => product.Price)
               .IsRequired();  

        builder.Property(product => product.Description)
               .HasMaxLength(1024); 

        builder.HasMany<CartItem>()  
               .WithOne(cartItem => cartItem.Product)
               .HasForeignKey(cartItem => cartItem.ProductId) 
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(product => product.CreatedDate)
               .IsRequired();
    }
}
