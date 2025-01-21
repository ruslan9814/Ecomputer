namespace Test.Endpoints.CartItems.Requests;

public sealed record UpdateCartItemRequest(int Id, int Quantity);