using Test.Models.Core;

namespace Test.Models;

public class Product : EntityBase
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public bool IsInStock { get; set; }
    public DateTime CreatedDate { get; set; }


    public Product()
    {
    }

    public Product(int id, string name, decimal price, int quantity, bool isInStock, DateTime createdDate, string? description = null) :
        base(id)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
        IsInStock = isInStock;
        Description = description;
        CreatedDate = createdDate;
    }
}


