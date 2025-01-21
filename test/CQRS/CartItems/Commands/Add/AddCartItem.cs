
using Test.Database.Repositories.Interfaces;

namespace test.CQRS.CartItems.Commands.Add;

public sealed record AddCartItem(
    int CartId, 
    int ProductId, 
    int Quantity
    ) : IRequest;


public sealed class AddCartItemHandler(
    ICartRepository cartRepository,
    IProductRepository productRepository) 
    : IRequestHandler<AddCartItem>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task Handle(AddCartItem request, CancellationToken cancellationToken)
    {
        var isProductExist = await _productRepository.IsExistAsync(request.ProductId);

        if (!isProductExist)
        {
            return;
        }

        var isCartExist = await _cartRepository.IsExistAsync(request.CartId);
        
        if (!isCartExist)
        {

        }

    }
}