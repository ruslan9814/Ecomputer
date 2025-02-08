using test.Common;
using test.Database.Service.UnitOfWork;
using Test.Database.Repositories.Interfaces;

namespace test.CQRS.CartItems.Commands;

public sealed record RemoveCartItemCommand(int Id) : IRequest<Result>;

public sealed class RemoveCartItemCommandHandler(
    ICartItemRepository cartItemRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveCartItemCommand, Result>
{
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
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

