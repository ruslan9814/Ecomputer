using Domain.Favorites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrasctructure.Database.Configurations;
public class FavoritesConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("Favorites");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.UserId)
               .IsRequired();

        builder.HasOne(f => f.User)
               .WithOne(u => u.Favorite)
               .HasForeignKey<Favorite>(f => f.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(f => f.FavoriteProducts)
               .WithOne(fp => fp.Favorite)
               .HasForeignKey(fp => fp.FavoriteId);

    }
}
