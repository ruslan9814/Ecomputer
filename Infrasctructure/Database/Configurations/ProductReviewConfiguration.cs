using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrasctructure.Database.Configurations;

public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.ToTable("ProductReviews");

        builder.HasKey(pr => pr.Id);

        builder.Property(pr => pr.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

       
        builder.HasIndex(pr => new { pr.ProductId, pr.UserId })
            .IsUnique();

        builder.Property(pr => pr.UserId)
            .IsRequired();

        builder.Property(pr => pr.ProductId)
            .IsRequired();

        builder.Property(pr => pr.ReviewText)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(pr => pr.Rating)
            .IsRequired();

        builder.Property(pr => pr.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
 
        builder.HasOne(pr => pr.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(pr => pr.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pr => pr.User)
            .WithMany(u => u.ProductReviews)
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}