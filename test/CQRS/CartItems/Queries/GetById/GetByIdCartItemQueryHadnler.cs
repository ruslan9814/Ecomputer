using test.CQRS.Dtos;
using Test.Database.Repositories.Interfaces;

namespace test.CQRS.CartItems.Queries.GetById;

public sealed record GetByIdCartItemQueryHadnler(ICartItemRepository CartItemRepository) 
    : IRequestHandler<GetByIdCartItemQuery, CartItemDto?>
{
    private readonly ICartItemRepository _cartItemRepository = CartItemRepository;

    public async Task<CartItemDto?> Handle(GetByIdCartItemQuery request, CancellationToken cancellationToken)
    {
        var isExist = await _cartItemRepository.IsExistAsync(request.Id);

        if (!isExist)
        {
            return null;
        }

        var cartItem = await _cartItemRepository.GetAsync(request.Id);

        var cartItemDto = new CartItemDto(
            cartItem.Id, 
            cartItem.Quantity,
            new ProductDto());


        return cartItemDto;
    }
}



