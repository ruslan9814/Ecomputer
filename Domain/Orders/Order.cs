
using Domain.Users;

namespace Domain.Orders;
public class Order : EntityBase
{
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public decimal TotalPrice => Items.Sum(x => x.Product.Price * x.Quantity);

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public int OrderStatusId { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public int UserId { get; set; }

    public User? User { get; set; } 

    private Order()
    {
    }

    public Order(int id) : base(id)
    {
    }

    public Order(int userId, User user, ICollection<OrderItem>? orderItems = null, 
        DateTime? createdDate = null, OrderStatus? status = null)
    {
        UserId = userId;
        User = user;
        Items = orderItems ?? new List<OrderItem>();
        CreatedDate = createdDate ?? DateTime.UtcNow;
        Status = status ?? OrderStatus.Pending;
    }

    public void UpdateStatus()
    {
        if (Status == OrderStatus.Pending && Items.Count > 0)
        {
            Status = OrderStatus.Processing;
        }
        else if (Status == OrderStatus.Processing)
        {
            var totalItems = Items.Count;
            var processedItems = Items.Count(x => x.Quantity > 0);

            if (totalItems == 0)
            {
                Status = OrderStatus.Cancelled;
            }
            else if (totalItems == processedItems)
            {
                Status = OrderStatus.Delivered;
            }
        }
    }

    public Result ChangeStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Cancelled || Status == OrderStatus.Delivered)
        {
            return Result.Failure("Статус не может быть изменён после завершения заказа.");
        }

        Status = newStatus;
        return Result.Success();
    }
}
