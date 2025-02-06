using test.Common;
using test.CQRS.Dtos;
using Test.Database.Repositories.Interfaces;

namespace test.CQRS.CartItems.Queries;

public sealed record GetByIdCartItemQuery(int Id) : IRequest<Result<CartItemDto>>;
public sealed record GetByIdCartItemQueryHandler(ICartItemRepository CartItemRepository)
    : IRequestHandler<GetByIdCartItemQuery, Result<CartItemDto>>
{
    private readonly ICartItemRepository _cartItemRepository = CartItemRepository;

    public async Task<Result<CartItemDto>> Handle(GetByIdCartItemQuery request, CancellationToken cancellationToken)
    {
        var isExist = await _cartItemRepository.IsExistAsync(request.Id);

        if (!isExist)
        {
            return Result<CartItemDto>
                .Failure($"Элемент корзины с ID {request.Id} не найден.");
        }

        var cartItem = await _cartItemRepository.GetAsync(request.Id);

        var response = new CartItemDto(
            cartItem.Id,
            cartItem.Quantity,
            new ProductDto(
                cartItem.ProductId,              
                cartItem.Product.Name,           
                cartItem.Product.Description,    
                cartItem.Product.Price,           
                cartItem.Product.IsInStock,       
                cartItem.Product.CreatedDate
                ));


        return Result<CartItemDto>.Success(response);
    }
}



