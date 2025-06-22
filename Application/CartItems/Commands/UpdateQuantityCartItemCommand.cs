using Domain.Users;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

public sealed record UpdateQuantityCartItemCommand(int Id, int Quantity) : IRequest<Result>;

internal sealed class UpdateQuantityCartItemCommandHandler(
    ICartItemRepository cartItemRepository,
    IProductRepository productRepository,
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser) : IRequestHandler<UpdateQuantityCartItemCommand, Result>
{
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(UpdateQuantityCartItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (userId <= 0)
        {
            return Result.Failure("Пользователь не авторизован.");
        }

        var cartItem = await _cartItemRepository.GetWithProductAndCartAsync(request.Id, userId, cancellationToken);
        if (cartItem is null)
        {
            return Result.Failure("Элемент корзины с таким ID не найден.");
        }

        var cart = await _cartRepository.GetAsync(cartItem.CartId);
        if (cart is null)
        {
            return Result.Failure("Корзина, связанная с этим элементом, не найдена.");
        }
 
        if (cart.UserId != _currentUser.UserId)
        {
            return Result.Failure("Доступ запрещён. Элемент не принадлежит вашей корзине.");
        }

        var result = cartItem.UpdateQuantity(cartItem.ProductId, request.Quantity);
        if (result.IsFailure)
        {
            return result;
        }

        await _cartItemRepository.UpdateAsync(cartItem);
        await _productRepository.UpdateAsync(cartItem.Product);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
