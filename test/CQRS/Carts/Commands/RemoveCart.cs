using test.Common;
using test.Database.DbService;
using Test.Database.Repositories.Interfaces;

public sealed record RemoveCartItem(int CartItemId) : IRequest<Result>;

public sealed class RemoveCartItemHandler(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IUnitOfWork unitOfWork) : IRequestHandler<RemoveCartItem, Result>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(RemoveCartItem request, CancellationToken cancellationToken)
    {
        var cartItem = await _cartItemRepository.GetAsync(request.CartItemId);

        if (cartItem is null)
        {
            return Result.Failure("Товар не найден в корзине.");
        }

        var cart = await _cartRepository.GetAsync(cartItem.CartId);
        if (cart is null)
        {
            return Result.Failure("Корзина не найдена.");
        }
       
        cart.Items.Remove(cartItem);
        cart.TotalSum = cart.Items.Sum(x => x.Product.Price * x.Quantity);

        await _cartRepository.UpdateAsync(cart);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}
