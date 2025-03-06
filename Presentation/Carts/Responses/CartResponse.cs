
using Presentation.CartItems.Responses;

namespace Infrasctructure.Endpoints.Carts.Responses;

public sealed record CartResponse(
    int Id, 
    int UserId,
    List<CartItemResponse> Items, decimal TotalSum);

