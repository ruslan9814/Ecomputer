using test.Common;
using test.Database.Service.UnitOfWork;
using Test.Database.Repositories.Interfaces;

namespace test.CQRS.CartItems.Commands;

public sealed record UpdateQuantityCartItemCommand(int Id, int Quantity) : IRequest<Result>;

public sealed class UpdateQuantityCartItemCommandHandler(
    ICartItemRepository cartItemRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateQuantityCartItemCommand, Result>
{
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateQuantityCartItemCommand request, CancellationToken cancellationToken)
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
