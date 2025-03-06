namespace Presentation.CartItems.Responses;

public sealed record CartItemResponse(int Id, int ProductId, string ProductName, int Quantity, decimal Price)
{
    public decimal TotalSum => Quantity * Price;
}
