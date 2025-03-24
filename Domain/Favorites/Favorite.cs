using Domain.Products;
using Domain.Users;

namespace Domain.Favorites;

public class Favorite : EntityBase
{
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<Product> Products { get; set; } = [];

    private Favorite() { }
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    public Favorite(int userId, ICollection<Product> products)
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    {
        UserId = userId;
        Products = products;
    }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    public Favorite(int userId)
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    {
        UserId = userId;
    }

    public Result AddProduct(Product product)
    {
        if (Products.Contains(product))
        {
            return Result.Failure("Товар уже добавлен в избранное.");
        }
        Products.Add(product);
        return Result.Success();
    }

    public Result RemoveProduct(int productId)
    {
        var productToRemove = Products.FirstOrDefault(p => p.Id == productId);
        if (productToRemove is null)
        {
            return Result.Failure("Товар не найден.");
        }
        Products.Remove(productToRemove);
        return Result.Success();
    }
}
