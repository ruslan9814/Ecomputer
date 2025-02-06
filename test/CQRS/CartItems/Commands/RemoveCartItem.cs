using test.Common;
using test.Database.DbService;
using Test.Database.Repositories.Interfaces;

namespace test.CQRS.CartItems.Commands;

public sealed record RemoveCartItem(int Id) : IRequest<Result>;

public sealed class RemoveCartItemHandler(
    ICartItemRepository cartItemRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveCartItem, Result>
{
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(RemoveCartItem request, CancellationToken cancellationToken)
    {
        var isCartItemExist = await _cartItemRepository.IsExistAsync(request.Id);
        if (!isCartItemExist)
        {
            return Result.Failure("Элемент корзины с таким ID не найден.");
        }

        await _cartItemRepository.DeleteAsync(request.Id);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}

