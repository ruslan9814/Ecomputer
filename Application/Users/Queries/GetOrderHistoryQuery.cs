using Application.Dtos;

namespace Application.Users.Queries;

public sealed record GetOrderHistoryQuery(int UserId) : IRequest<IEnumerable<OrderDto>>;

