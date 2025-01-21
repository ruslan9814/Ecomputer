using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Models;

namespace Test.Database.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
      
        builder.ToTable("Carts");

        builder.HasKey(cart => cart.Id);

        builder.Property(cart => cart.UserId)
               .IsRequired();

        builder.HasMany(cart => cart.Items)
               .WithOne()
               .HasForeignKey(cartItem => cartItem.CartId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cart => cart.UserId);

        builder.Property(cart => cart.TotalSum)
               .IsRequired();
    }
}
