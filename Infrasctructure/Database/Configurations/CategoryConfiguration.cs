using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace Infrasctructure.Database.Configurations;
public class CategoryConfiguration
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
    }
}
