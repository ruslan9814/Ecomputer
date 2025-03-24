using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrasctructure.Database.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(order => order.Id);
        builder.Property(order => order.UserId)
               .IsRequired();
        builder.Property(order => order.OrderStatusId)
               .IsRequired();
        //builder.HasOne(order => order.Status)
        //       .WithMany()
        //       .HasForeignKey(order => order.OrderStatusId)
        //       .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(order => order.Items)
               .WithOne()
               .HasForeignKey(orderItem => orderItem.Id)
               .OnDelete(DeleteBehavior.Cascade);
        builder.Ignore(order => order.TotalPrice);
    }
 }
