using Domain.Favorites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasctructure.Database.Configurations;

public class FavoriteProductConfiguration : IEntityTypeConfiguration<FavoriteProduct>
{
    public void Configure(EntityTypeBuilder<FavoriteProduct> builder)
    {
        builder.ToTable("FavoriteProducts");

        builder.HasKey(fp => new { fp.FavoriteId, fp.ProductId });

        builder.HasOne(fp => fp.Favorite)
               .WithMany(f => f.FavoriteProducts)
               .HasForeignKey(fp => fp.FavoriteId);

        builder.HasOne(fp => fp.Product)
               .WithMany()
               .HasForeignKey(fp => fp.ProductId);
    }
}
