using Application.Dtos;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;

namespace Application.CartItems.Queries;

public sealed record GetByIdCartItemQuery(int Id) : IRequest<Result<CartItemDto>>;


internal sealed record GetByIdCartItemQueryHandler(
    ICartItemRepository CartItemRepository,
    ICurrentUserService CurrentUser)
    : IRequestHandler<GetByIdCartItemQuery, Result<CartItemDto>>
{
    private readonly ICartItemRepository _cartItemRepository = CartItemRepository;
    private readonly ICurrentUserService _currentUser = CurrentUser;

    public async Task<Result<CartItemDto>> Handle(GetByIdCartItemQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (userId <= 0)
        {
            return Result.Failure<CartItemDto>("Пользователь не авторизован.");
        }

        var cartItem = await _cartItemRepository.GetWithProductAndCartAsync(request.Id, userId, cancellationToken);

        if (cartItem is null)
        {
            var exists = await _cartItemRepository.ExistsAsync(request.Id, cancellationToken);

            if (exists)
            {
                return Result.Failure<CartItemDto>("Доступ запрещён: элемент корзины не принадлежит текущему пользователю.");
            }
            else
            {
                return Result.Failure<CartItemDto>($"Элемент корзины с ID {request.Id} не найден.");
            }
        }

        if (cartItem.Product is null)
        {
            return Result.Failure<CartItemDto>($"Продукт для элемента корзины с ID {request.Id} не найден.");
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
                cartItem.Product.Quantity,
                cartItem.Product.CategoryId,
                cartItem.Product.Category?.Name ?? string.Empty,
                cartItem.Product.Rating,
                cartItem.Product.ImageUrl));

        return Result.Success(response);
    }
}


