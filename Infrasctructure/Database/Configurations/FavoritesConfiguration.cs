using Domain.Favorites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrasctructure.Database.Configurations;

public class FavoritesConfiguration
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("Favorites");
        builder.HasKey(favorites => favorites.Id);
        builder.Property(favorites => favorites.UserId_FK)
               .IsRequired();
        builder.HasOne(favorites => favorites.User)
               .WithMany()
               .HasForeignKey(favorites => favorites.UserId_FK)
               .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(favorites => favorites.Products)
               .WithOne()
               .HasForeignKey(product => product.Id)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
