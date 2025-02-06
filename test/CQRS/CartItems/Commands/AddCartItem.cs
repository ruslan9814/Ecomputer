using test.Common;
using test.Database.DbService;
using Test.Database.Repositories.Interfaces;
using Test.Models;

namespace test.CQRS.CartItems.Commands;

public sealed record AddCartItem(
    int CartId,
    int ProductId,
    int Quantity
    ) : IRequest<Result>;


public sealed class AddCartItemHandler(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    ICartItemRepository cartItemRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddCartItem, Result>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddCartItem request, CancellationToken cancellationToken)
    {
        var isProductExist = await _productRepository.IsExistAsync(request.ProductId);

        if (!isProductExist)
        {
            return Result.Failure("Продукт не существует.");
        }

        var isCartExist = await _cartRepository.IsExistAsync(request.CartId);

        if (!isCartExist)
        {
            return Result.Failure("Корзины не существует.");
        }

        var cartItem = new CartItem
        {
            CartId = request.CartId,
            ProductId = request.ProductId,
            Quantity = request.Quantity
        };

        await _cartItemRepository.AddAsync(cartItem);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}

