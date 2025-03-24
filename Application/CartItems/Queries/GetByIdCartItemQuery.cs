using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.CartItems.Queries;

public sealed record GetByIdCartItemQuery(int Id) : IRequest<Result<CartItemDto>>;
internal sealed record GetByIdCartItemQueryHandler(ICartItemRepository CartItemRepository)
    : IRequestHandler<GetByIdCartItemQuery, Result<CartItemDto>>
{
    private readonly ICartItemRepository _cartItemRepository = CartItemRepository;

    public async Task<Result<CartItemDto>> Handle(GetByIdCartItemQuery request, CancellationToken cancellationToken)
    {
        var isExist = await _cartItemRepository.IsExistAsync(request.Id);

        if (!isExist)
        {
            return Result.Failure<CartItemDto>(
                $"Элемент корзины с ID {request.Id} не найден.");
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
                cartItem.Product.CreatedDate,
                new CategoryDto(cartItem.Product.Category.Id, cartItem.Product.Category.Name)
                ));


        return Result.Success(response);
    }
}



