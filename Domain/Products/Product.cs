using Domain.Categories;
using Domain.Coupon;
using Domain.Coupons;
using Domain.Favorites;
using System.Text.Json.Serialization;

namespace Domain.Products;

public class Product : EntityBase
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool IsInStock { get; set; }
    public float Rating { get; set; } 
    public int CategoryId { get; set; }
    public string? ImageUrl { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }
 

    [JsonIgnore]
    public ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();
    public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
    public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
    public ICollection<PromoCode> Coupons { get; set; } = new List<PromoCode>();
    public DateTime CreatedDate { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private Product()
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    {
        CreatedDate = DateTime.UtcNow;
    }

    [JsonConstructor]
    public Product(string name, decimal price, int quantity, bool isInStock,
        Category category, string description = null!, string? imageUrl = null) : base(0)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        IsInStock = isInStock;
        Description = description ?? string.Empty;
        CreatedDate = DateTime.UtcNow;
        Category = category;
        CategoryId = category.Id;
        ImageUrl = imageUrl;
    }

    public Result Update(string name, decimal price, int quantity, bool isInStock,
        string description, Category category, string imageUrl)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        UpdateStockStatus();
        Description = description ?? string.Empty;
        Category = category;
        CategoryId = category.Id;
        ImageUrl = imageUrl;  
        return Result.Success();
    }

    public Result DecreaseQuantity(int quantity)
    {
        if (quantity > Quantity)
            return Result.Failure("Not enough quantity");

        Quantity -= quantity;
        UpdateStockStatus();

        return Result.Success();
    }

    public Result IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
        UpdateStockStatus();
        return Result.Success();
    }

    private void UpdateStockStatus()
    {
        IsInStock = Quantity > 0;
    }

    public Result RecalculateRating(IEnumerable<float> ratings)
    {
        if (ratings is null || !ratings.Any())
        {
            Rating = 0;
            return Result.Success();
        }

        var averageRating = (float)Math.Round(ratings.Average(), 1);

        if (averageRating < 0 || averageRating > 5)
        {
            return Result.Failure("Rating must be between 0 and 5.");
        }

        Rating = averageRating;
        return Result.Success();
    }

}
