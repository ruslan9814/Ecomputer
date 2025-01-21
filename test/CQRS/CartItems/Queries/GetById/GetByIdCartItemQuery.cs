using test.CQRS.Dtos;

namespace test.CQRS.CartItems.Queries.GetById;

public sealed record GetByIdCartItemQuery(int Id) 
    : IRequest<Result<CartItemDto>>;



