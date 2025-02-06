using test.Common;
using test.Database.DbService;
using Test.Database.Repositories.Interfaces;

namespace test.CQRS.CartItems.Commands;

public sealed record UpdateQuantityCartItem(int Id, int Quantity) : IRequest<Result>;

public sealed class UpdateQuantityCartItemHandler(
    ICartItemRepository cartItemRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateQuantityCartItem, Result>
{
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateQuantityCartItem request, CancellationToken cancellationToken)
    {
        var isExist = await _cartItemRepository.IsExistAsync(request.Id);

        if (!isExist)
        {
            return Result.Failure("Элемент корзины с таким ID не найден.");
        }

        var cartItem = await _cartItemRepository.GetAsync(request.Id);

        cartItem.UpdateQuantity(request.Quantity);

        await _cartItemRepository.UpdateAsync(cartItem);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}
