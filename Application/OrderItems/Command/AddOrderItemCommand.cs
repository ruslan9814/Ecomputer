using Domain.Orders;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.OrderItems.Command;


public sealed record AddOrderItemCommand(int CartId) : IRequest<Result>;

internal sealed class AddOrderItemCommandHandler(
    ICartRepository cartRepository,
    ICartItemRepository cartItemRepository,
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser) : IRequestHandler<AddOrderItemCommand, Result>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var user = await _userRepository.GetAsync(userId);
        if (user is null)
        {
            return Result.Failure("Пользователь не найден.");
        }

        var cart = await _cartRepository.GetWithItemsAsync(request.CartId);
        if (cart is null || cart.Items.Count == 0)
        {
            return Result.Failure("Корзина пуста.");
        }

        if (cart.UserId != userId)
        {
            return Result.Failure("Доступ запрещён: корзина не принадлежит пользователю.");
        }

        var order = new Order(userId, user);

        foreach (var cartItem in cart.Items)
        {
            var product = await _productRepository.GetAsync(cartItem.ProductId);
            if (product is null)
            {
                return Result.Failure($"Продукт с ID {cartItem.ProductId} не найден.");
            }

            var orderItem = new OrderItem(
                product.Id,
                product,
                order.Id,
                order,
                cartItem.Quantity,
                product.Price
            );

            order.Items.Add(orderItem);
            await _productRepository.UpdateAsync(product);
        }

        order.UpdateStatus();

        await _orderRepository.AddAsync(order);
        await _cartItemRepository.RemoveRangeAsync(cart.Items, userId);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
