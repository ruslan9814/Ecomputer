using test.Common;
using test.Database.DbService;
using Test.Database.Repositories.Interfaces;

public sealed record UpdateQuantityCartItem(int CartItemId, int Quantity) : IRequest<Result>;

public sealed class UpdateQuantityCartItemHandler(ICartItemRepository cartItemRepository, ICartRepository cartRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateQuantityCartItem, Result>
{
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateQuantityCartItem request, CancellationToken cancellationToken)
    {
        var cartItem = await _cartItemRepository.GetAsync(request.CartItemId);

        if (cartItem == null)
        {
            return Result.Failure("Товар не найден в корзине.");
        }

        cartItem.Quantity = request.Quantity;

        var cart = await _cartRepository.GetAsync(cartItem.CartId);
        if (cart == null)
        {
            return Result.Failure("Корзина не найдена.");
        }

        cart.TotalSum = cart.Items.Sum(x => x.Product.Price * x.Quantity);

        await _cartItemRepository.UpdateAsync(cartItem); 
        await _cartRepository.UpdateAsync(cart); 
        await _unitOfWork.Commit();

        return Result.Success;
    }
}
