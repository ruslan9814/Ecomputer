using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Carts;

namespace Infrasctructure.Database.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {

        builder.ToTable("CartItems");

        builder.HasKey(cartItem => cartItem.Id);

        builder.HasOne(cartItem => cartItem.Cart)
               .WithMany(cart => cart.Items)  
               .HasForeignKey(cartItem => cartItem.CartId) 
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cartItem => cartItem.Product)
               .WithMany()
               .HasForeignKey(cartItem => cartItem.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(cartItem => cartItem.Quantity)
               .IsRequired();

        builder.Property(cartItem => cartItem.ProductId)
               .IsRequired();

        builder.Property(cartItem => cartItem.CartId)
               .IsRequired();
    }
}

