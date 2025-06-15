using Domain.Products;
using Domain.Users;
using System.Text.Json.Serialization;

namespace Domain.Favorites;

public class Favorite : EntityBase
{
    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    public ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();

    private Favorite()
    {
    }

    public Favorite(int id, int UserId) : base(id)
    {
        this.UserId = UserId;
    }

    public Favorite(User user) : base(0)
    {
        User = user;
        UserId = user.Id;
    }

    public Result AddProduct(Product product)
    {
        if (FavoriteProducts.Any(fp => fp.ProductId == product.Id))
        {
            return Result.Failure("Товар уже добавлен в избранное.");
        }

        FavoriteProducts.Add(new FavoriteProduct
        {
            ProductId = product.Id,
            Product = product,
            Favorite = this
        });

        return Result.Success();
    }

    public Result RemoveProduct(int productId)
    {
        var item = FavoriteProducts.FirstOrDefault(fp => fp.ProductId == productId);
        if (item is null)
        {
            return Result.Failure("Товар не найден.");
        }

        FavoriteProducts.Remove(item);
        return Result.Success();
    }

    public bool ContainsProduct(int productId)
    {
        return FavoriteProducts.Any(fp => fp.ProductId == productId);
    }
}
