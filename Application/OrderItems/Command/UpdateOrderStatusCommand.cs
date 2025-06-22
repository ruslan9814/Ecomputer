using Domain.Orders;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.OrderItems.Command;

public sealed record UpdateOrderStatusCommand(int OrderId, OrderStatus NewStatus) 
    : IRequest<Result>;

internal sealed class UpdateOrderStatusCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateOrderStatusCommand, Result>
{
    private readonly IOrderRepository orderRepository = orderRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateOrderStatusCommand request, 
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId);
        if (order is null)
        {
            return Result.Failure("Заказ не найден.");
        }

        var result = order.ChangeStatus(request.NewStatus);
        if (result.IsFailure)
        {
            return result;
        }

        await orderRepository.UpdateAsync(order);
        await unitOfWork.Commit();

        return Result.Success();
    }
}
