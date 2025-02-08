using test.Common;
using test.Database.Service.UnitOfWork;
using Test.Database.Repositories.Interfaces;

public sealed record ClearCartCommand(int Id) : IRequest<Result>;

public sealed class ClearCartCommandHandler(
    ICartRepository cartRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<ClearCartCommand, Result>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cartIsExist = await _cartRepository.IsExistAsync(request.Id);

        if (!cartIsExist)
        {
            return Result.Failure("Корзина не найдена.");
        }

        var cart = await _cartRepository.GetAsync(request.Id);

        cart.Items.Clear();
        await _cartRepository.UpdateAsync(cart);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}
