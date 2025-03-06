using Domain.Categories;

namespace Domain.Products;

public class Product : EntityBase
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public bool IsInStock { get; set; }
    public Category Category { get; set; }
    public DateTime CreatedDate { get; set; }


#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private Product() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.

    public Product(int id, string name, decimal price, int quantity, bool isInStock,
        DateTime createdDate, Category category, string description = null!) : base(id)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        IsInStock = isInStock;
        Description = description;
        CreatedDate = createdDate;
        Category = category;
    }

    public Product(string name, decimal price, int quantity, bool isInStock,
        DateTime createdDate, Category category, string description = null!)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        IsInStock = isInStock;
        Description = description;
        CreatedDate = createdDate;
        Category = category;
    }

    public Result Update(string name, decimal price, int quantity, bool isInStock, string description, Category category)
    {

        Name = name;
        Price = price;
        Quantity = quantity;
        IsInStock = isInStock;
        Description = description;
        Category = category;
        return Result.Success();
    }
}


