using test.CQRS.Dtos;

namespace test.CQRS.Users.Commands;

public sealed record GetOrderHistory(int UserId) : IRequest<IEnumerable<OrderDto>>;

