using Application.Dtos;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Carts.Queries;

public sealed record GetCartQuery() : IRequest<Result<CartDto>>;

internal sealed class GetCartQueryHandler(ICartRepository cartRepository, ICurrentUserService currentUser)
    : IRequestHandler<GetCartQuery, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result<CartDto>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (userId <= 0)
        {
            return Result.Failure<CartDto>("Пользователь не авторизован.");
        }
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart is null)
        {
            return Result.Failure<CartDto>("Корзина не найдена.");
        }

        var cartItemsDto = cart.Items
            .Where(ci => ci.Product is not null)
            .Select(ci =>
            {
                var product = ci.Product!;
                return new CartItemDto(
                    ci.Id,
                    ci.Quantity,
                    new ProductDto(
                        product.Id,
                        product.Name,
                        product.Description,
                        product.Price,
                        product.IsInStock,
                        product.CreatedDate,
                        product.Quantity,
                        product.CategoryId,
                        product.Category?.Name ?? string.Empty,
                        product.Rating,
                        product.ImageUrl
                    )
                );
            })
            .ToList();

        var totalPrice = cartItemsDto.Sum(i => i.Quantity * i.Product.Price);

        var cartDto = new CartDto(
            cart.UserId,
            totalPrice,
            cartItemsDto
        );

        return Result.Success(cartDto);
    }
}
