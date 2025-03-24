using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;


namespace Application.Carts.Commands;

public sealed record ClearCartCommand(int Id) : IRequest<Result>;

internal sealed class ClearCartCommandHandler(
    ICartRepository cartRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<ClearCartCommand, Result>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetWithItemsAsync(request.Id);

        if (cart is null)
        {
            return Result.Failure("Корзина не найдена.");
        }

        if (cart.Items.Count != 0)
        {
            await _cartRepository.RemoveItemsAsync(cart.Id);
            await _unitOfWork.Commit();
        }

        return Result.Success();

    }
}
