using Domain.Products;
using System.Text.Json.Serialization;

namespace Domain.Categories;

public class Category : EntityBase
{
    public string Name { get; set; } = null!;
    public ICollection<Product> Products { get; set; } = [];

    private Category()
    {
    }

    [JsonConstructor]
    public Category(string name) : base(0)
    {
        Name = name;
    }

}