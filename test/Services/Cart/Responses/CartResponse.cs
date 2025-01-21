using Test.Endpoints.CartItems.Responses;

namespace test.Services.CartItem.Responses;

public sealed record CartResponse(int Id, int UserId, List<CartItemResponse> Items, decimal TotalSum);
