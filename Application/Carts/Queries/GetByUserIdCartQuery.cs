using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Carts.Queries;

public sealed record GetByUserIdCartQuery(int UserId) : IRequest<Result<CartDto>>;

internal sealed class GetByUserIdCartQueryHandler(ICartRepository cartRepository) 
    : IRequestHandler<GetByUserIdCartQuery, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    public async Task<Result<CartDto>> Handle(GetByUserIdCartQuery request, 
        CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAsync(request.UserId);

        if (cart is null)
        {
            return Result.Failure<CartDto>("Корзина не найдена.");
        }

        var cartItemsDto = new List<CartItemDto>();
        decimal totalPrice = 0;

        foreach (var cartItem in cart.Items)
        {
            var product = cartItem.Product;
            var cartItemDto = new CartItemDto(
                cartItem.Id,
                cartItem.Quantity,
                new ProductDto(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.IsInStock,
                    product.CreatedDate,
                    product.CategoryId
                )
            );

            cartItemsDto.Add(cartItemDto);
            totalPrice += cartItem.Quantity * product.Price;
        }

        var cartDto = new CartDto(
            cart.UserId,
            totalPrice,
            cartItemsDto
        );

        return Result.Success(cartDto);
    }
}
