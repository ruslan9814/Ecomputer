namespace Presentation.CartItems.Requests;

public sealed record AddCartItemRequest(int CartId, int ProductId, int Quantity);
