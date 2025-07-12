using Domain.Carts;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.CartItems.Commands;

public sealed record AddCartItemCommand(
    int CartId,
    int ProductId,
    int Quantity
) : IRequest<Result>;

internal sealed class AddCartItemCommandHandler(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    ICartItemRepository cartItemRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser)
    : IRequestHandler<AddCartItemCommand, Result>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(request.CartId);
        if (cart is null)
        {
            return Result.Failure("Корзина не существует.");
        }

        if (cart.UserId != _currentUser.UserId)
        {
            return Result.Failure("Доступ запрещён. Корзина принадлежит другому пользователю.");
        }

        var product = await _productRepository.GetAsync(request.ProductId);
        if (product is null)
        {
            return Result.Failure("Продукта не существует.");
        }

        var existingCartItem = await _cartItemRepository
            .GetByCartAndProductAsync(request.CartId, request.ProductId);

        if (existingCartItem is not null)
        {
            return Result.Failure("Товар уже добавлен в корзину.");
        }

        var decreaseResult = product.DecreaseQuantity(request.Quantity);
        if (decreaseResult.IsFailure)
        {
            return Result.Failure($"Недостаточно товара на складе. Доступно: {product.Quantity}");
        }

        var cartItem = new CartItem(
            id: 0,  
            quantity: request.Quantity,
            productId: product.Id,
            product: product,
            cartId: cart.Id,
            cart: cart
        );


        await _cartItemRepository.AddAsync(cartItem);
        await _productRepository.UpdateAsync(product);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}

