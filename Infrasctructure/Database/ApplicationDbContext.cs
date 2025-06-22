
using Domain.Carts;
using Domain.Categories;
using Domain.Coupon;
using Domain.Coupons;
using Domain.Favorites;
using Domain.Orders;
using Domain.Payments;
using Domain.Products;
using Domain.Promotions;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrasctructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Coupon> Coupons { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql();
    protected override void OnModelCreating(ModelBuilder modelBuilder) => 
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
}
