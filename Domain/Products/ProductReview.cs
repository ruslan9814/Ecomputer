using Domain.Users;

namespace Domain.Products;

public class ProductReview : EntityBase
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public float Rating { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual Product? Product { get; set; }
    public virtual User? User { get; set; }

    private ProductReview() { }

    public ProductReview(int id, int productId, int userId, string reviewText, float rating)
        : base(id)
    {
        ProductId = productId;
        UserId = userId;
        ReviewText = reviewText ?? string.Empty;
        Rating = Math.Clamp(rating, 0, 5);
        CreatedAt = DateTime.UtcNow;
    }
}
