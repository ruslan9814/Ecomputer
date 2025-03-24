using Domain.Carts;
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
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddCartItemCommand, Result>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddCartItemCommand request, 
        CancellationToken cancellationToken)
    {
        var isProductExist = await _productRepository.IsExistAsync(request.ProductId);


        if (!isProductExist)
        {
            return Result.Failure("Продукта не существует.");
        }

        var isCartExist = await _cartRepository.IsExistAsync(request.CartId);

        if (!isCartExist)
        {
            return Result.Failure("Корзина не существует");

        }

        var cartItem = new CartItem
        {
            CartId = request.CartId,
            ProductId = request.ProductId,
            Quantity = request.Quantity
        };

        await _cartItemRepository.AddAsync(cartItem);
        await _unitOfWork.Commit();

        return Result.Success();
    }


}

