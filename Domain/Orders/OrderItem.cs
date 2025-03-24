using Domain.Products;

namespace Domain.Orders;

public class OrderItem : EntityBase
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private OrderItem() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    public OrderItem(int productId, Product product, int orderId,  Order order, int quantity, decimal price)
    {
        ProductId = productId;
        Product = product;
        OrderId = orderId;
        Order = order;
        Quantity = quantity;
        Price = price;
    }

    public Result AddProductToOrderItem(int orderId, int productId, int quantity, decimal price)
    {
        if (Order is null || OrderId != orderId)
        {
            return Result.Failure("Заказ не найден.");
        }

        if (Product is null || ProductId != productId)
        {
            return Result.Failure("Продукт не найден.");
        }

        if (quantity <= 0)
        {
            return Result.Failure("Количество продукта должно быть больше нуля.");
        }

        if (price <= 0)
        {
            return Result.Failure("Цена продукта должна быть больше нуля.");
        }
        var orderItem = Order.Items.FirstOrDefault(x => x.ProductId == productId);
        if (orderItem is not null)
        {
            orderItem.Quantity += quantity;
            orderItem.Price = price;
        }
        else
        {
            Quantity = quantity;
            Price = price;
            Order.Items.Add(this);
        }
        return Result.Success();
    }

    public Result RemoveProductFromOrderItem(int orderId, int productId)
    {
        if (Order is null || OrderId != orderId)
        {
            return Result.Failure("Заказ не найден.");
        }
        if (Product is null || ProductId != productId)
        {
            return Result.Failure("Продукт не найден.");
        }
        var orderItem = Order.Items.FirstOrDefault(x => x.ProductId == productId);

        if (orderItem is null)
        {
            return Result.Failure("Продукт не найден в заказе.");
        }

        if (orderItem.Quantity > 0)
        {
            orderItem.Quantity--;
        }
        else
        {
            Order.Items.Remove(orderItem);
        }
        return Result.Success();
    }
}
