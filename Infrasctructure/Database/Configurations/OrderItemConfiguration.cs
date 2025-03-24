using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrasctructure.Database.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(orderItem => orderItem.Id);
        builder.HasOne(orderItem => orderItem.Order)
               .WithMany(order => order.Items)
               .HasForeignKey(orderItem => orderItem.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(orderItem => orderItem.Product)
               .WithMany()
               .HasForeignKey(orderItem => orderItem.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
        builder.Property(orderItem => orderItem.Quantity)
               .IsRequired();
        builder.Property(orderItem => orderItem.ProductId)
               .IsRequired();
        builder.Property(orderItem => orderItem.OrderId)
               .IsRequired();
    }
}
