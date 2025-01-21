namespace Test.Endpoints.CartItems.Requests;

public sealed record AddCartItemRequest(int CartId, int ProductId, int Quantity);
