using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.CartItems.Queries;

public sealed record GetByIdCartItemQuery(int Id) : IRequest<Result<CartItemDto>>;
internal sealed record GetByIdCartItemQueryHandler(ICartItemRepository CartItemRepository)
    : IRequestHandler<GetByIdCartItemQuery, Result<CartItemDto>>
{
    private readonly ICartItemRepository _cartItemRepository = CartItemRepository;

    public async Task<Result<CartItemDto>> Handle(GetByIdCartItemQuery request, 
        CancellationToken cancellationToken)
    {

        var cartItem = await _cartItemRepository.GetWithProductAsync(request.Id);

        if (cartItem is null)
        {
            return Result.Failure<CartItemDto>(
                $"Элемент корзины с ID {request.Id} не найден.");
        }

        if (cartItem.Product is null)
        {
            return Result.Failure<CartItemDto>(
                $"Продукт для элемента корзины с ID {request.Id} не найден.");
        }

        var response = new CartItemDto(
            cartItem.Id,
            cartItem.Quantity,
            new ProductDto(
                cartItem.ProductId,
                cartItem.Product.Name ?? string.Empty, 
                cartItem.Product.Description ?? string.Empty,
                cartItem.Product.Price,
                cartItem.Product.IsInStock,
                cartItem.Product.CreatedDate,
                cartItem.Product.CategoryId));

        return Result.Success(response);
    }
}



