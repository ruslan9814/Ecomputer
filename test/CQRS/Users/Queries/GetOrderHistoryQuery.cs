using test.CQRS.Dtos;

namespace test.CQRS.Users.Queries;

public sealed record GetOrderHistoryQuery(int UserId) : IRequest<IEnumerable<OrderDto>>;

