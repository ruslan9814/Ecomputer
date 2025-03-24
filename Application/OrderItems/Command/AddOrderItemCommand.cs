using Domain.Orders;
using Infrasctructure.UnitOfWork;
using Infrasctructure.Repositories.Interfaces;

namespace Application.OrderItems.Command;

public sealed record AddOrderItemCommand(
    int OrderId,
    int ProductId,
    int Quantity,
    decimal Price
) : IRequest<Result>;

internal sealed class AddOrderItemCommandHandler(IOrderRepository orderRepository, 
    IProductRepository productRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<AddOrderItemCommand, Result>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddOrderItemCommand request, 
        CancellationToken cancellationToken)
    {

        var orderIsExists = await _orderRepository.IsExistAsync(request.OrderId);
        if (!orderIsExists)
        {
            return Result.Failure("Заказ не найден.");
        }

        var productIsExists = await _productRepository.IsExistAsync(request.OrderId);
        if (!productIsExists)
        {
            return Result.Failure("Продукт не найден.");
        }

        var product = await _productRepository.GetAsync(request.ProductId);
        var order = await _orderRepository.GetAsync(request.OrderId);

     
        var orderItem = order.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (orderItem is not null)
        {
            var result = orderItem.AddProductToOrderItem(request.OrderId, request.ProductId, 
                request.Quantity, request.Price);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error!);
            }
        }
        else
        {
            var newOrderItem = new OrderItem(product.Id, product,
            order.Id, order, request.Quantity, product.Price);
            order.Items.Add(newOrderItem);
        }
        // TODO: change logic add method in order item repository to get by order id and product id
        // add logic with change status of order
        order.UpdateStatus();
        await _orderRepository.UpdateAsync(order);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
