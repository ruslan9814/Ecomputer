using Domain.Carts;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Classes;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.CartItems.Commands;

public sealed record RemoveCartItemCommand(int Id) : IRequest<Result>;

internal sealed class RemoveCartItemCommandHandler(
    ICartItemRepository cartItemRepository,
    IProductRepository productRepository,
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser) : IRequestHandler<RemoveCartItemCommand, Result>
{
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var cartItem = await _cartItemRepository.GetAsync(request.Id);
        if (cartItem is null)
        {
            return Result.Failure("Элемент корзины с таким ID не найден.");
        }

        var cart = await _cartRepository.GetAsync(cartItem.CartId);
        if (cart is null)
        {
            return Result.Failure("Корзина, связанная с элементом, не найдена.");
        }

 
        if (cart.UserId != _currentUser.UserId)
        {
            return Result.Failure("Доступ запрещён. Элемент не принадлежит вашей корзине.");
        }

        var product = await _productRepository.GetAsync(cartItem.ProductId);
        if (product is null)
        {
            return Result.Failure("Продукт, связанный с этим элементом, не найден.");
        }

        var increaseResult = product.IncreaseQuantity(cartItem.Quantity);
        if (increaseResult.IsFailure)
        {
            return Result.Failure($"Не удалось восстановить товар на складе: {increaseResult.Error}");
        }

        await _productRepository.UpdateAsync(product);
        await _cartItemRepository.DeleteAsync(cartItem.Id);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
