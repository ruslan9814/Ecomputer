using Domain.Orders;


namespace Domain.Payments;

public class Payment : EntityBase
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime PaidAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public Payment() { }
    public Payment(int id, int orderId, decimal amount, string paymentMethod, string status, DateTime paidAt) : base(id)
    {
        OrderId = orderId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Status = status;
        PaidAt = paidAt;
    }
}

