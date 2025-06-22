using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries;

public sealed record GetTopProductsQuery(int TopCount = 3)
    : IRequest<Result<IEnumerable<object>>>;

internal sealed class GetTopProductsQueryHandler(
    IOrderRepository orderRepository)
    : IRequestHandler<GetTopProductsQuery, Result<IEnumerable<object>>>
{
    public async Task<Result<IEnumerable<object>>> Handle(GetTopProductsQuery request, 
        CancellationToken cancellationToken)
    {
        var topProducts = await orderRepository.GetTopProductsAsync(request.TopCount);
        return Result.Success(topProducts);
    }
}
