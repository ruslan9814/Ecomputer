using Test.Endpoints.CartItems.Responses;
using Test.Models;

namespace Test.Endpoints.Carts.Responses;

public sealed record CartResponse(int Id, int UserId, List<CartItemResponse> Items, decimal TotalSum);

