using Domain.Products;
using System.Text.Json.Serialization;

namespace Domain.Categories;

public class Category : EntityBase
{
    public string Name { get; private set; }

    public ICollection<Product> Products { get; set; } = [];

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private Category() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.


    [JsonConstructor]
    public Category(string name) : base(0)
    {
        Name = name;

    }
}
