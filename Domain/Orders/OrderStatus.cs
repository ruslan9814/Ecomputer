namespace Domain.Orders;

public enum OrderStatus : sbyte
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
