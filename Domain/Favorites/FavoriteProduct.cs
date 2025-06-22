using Domain.Products;

namespace Domain.Favorites;

public class FavoriteProduct
{
    public int FavoriteId { get; set; }
    public Favorite? Favorite { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }
}


