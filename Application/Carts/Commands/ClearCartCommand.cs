using Application.Dtos;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Carts.Commands;

public sealed record ClearCartCommand() : IRequest<Result>;

internal sealed class ClearCartCommandHandler(
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork,
    IProductRepository productRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<ClearCartCommand, Result>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Result> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId <= 0)
        {
            return Result.Failure("Пользователь не авторизован.");
        }

        var cart = await _cartRepository.GetWithItemsAndProductsAsync(userId);
        if (cart is null)
        {
            return Result.Failure("Корзина не найдена.");
        }

        if (cart.Items.Count == 0)
        {
            return Result.Success(); 
        }

        foreach (var item in cart.Items.Where(i => i.Product is not null))
        {
            var product = await _productRepository.GetAsync(item.Product.Id);
            if (product is null)
            {
                Console.WriteLine($"Продукт с ID {item.Product.Id} не найден. Пропущен.");
                continue;
            }

            var result = product.IncreaseQuantity(item.Quantity);
            if (result.IsFailure)
            {
                Console.WriteLine($"Ошибка при увеличении количества товара {product.Id}: {result.Error}");
                continue;
            }

            await _productRepository.UpdateAsync(product);
        }

        await _cartRepository.RemoveItemsAsync(cart.Id);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
