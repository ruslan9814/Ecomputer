using Domain.Users;

namespace Domain.Orders;

public class Order : EntityBase
{
    public ICollection<OrderItem> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(x => x.Product.Price * x.Quantity);
    public DateTime CreatedDate { get; set; }
    public int OrderStatusId { get; set; }
    public OrderStatus Status { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private Order() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    public Order(int userId, User user, ICollection<OrderItem> orderItems, DateTime createdDate, OrderStatus status)
    {
        UserId = userId;
        User = user;
        Items = orderItems;
        CreatedDate = createdDate;
        Status = status;
    }
    public Order(int userId, User user, DateTime createdDate, OrderStatus status)
    {
        UserId = userId;
        User = user;
        CreatedDate = createdDate;
        Status = status;
    }

    public void UpdateStatus()
    {
        if (Status == OrderStatus.Pending && Items.Count > 0)
        {
            Status = OrderStatus.Processing;
        }

        if (Status == OrderStatus.Processing)
        {
            var totalItems = Items.Count;
            var processedItems = Items.Count(x => x.Quantity > 0);

            if (totalItems == processedItems)
            {
                Status = OrderStatus.Delivered;
            }
        }

        if (Status == OrderStatus.Processing && Items.Count == 0)
        {
            Status = OrderStatus.Cancelled;
        }
    }
}
