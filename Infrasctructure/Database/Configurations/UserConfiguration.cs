using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Users;
using Domain.Carts;
using Domain.Favorites;

namespace Infrasctructure.Database.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(user => user.Email)
               .IsUnique();

        builder.Property(user => user.Email)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasOne(user => user.Cart)
               .WithOne(cart => cart.User)
               .HasForeignKey<Cart>(cart => cart.UserId);

        builder.HasOne(user => user.Favorite)
                .WithOne(favorite => favorite.User)
                .HasForeignKey<Favorite>(favorite => favorite.UserId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
