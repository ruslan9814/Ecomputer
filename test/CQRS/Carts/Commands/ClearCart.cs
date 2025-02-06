using test.Common;
using test.Database.DbService;
using Test.Database.Repositories.Interfaces;

public sealed record ClearCart(int UserId) : IRequest<Result>;

public sealed class ClearCartHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork) : IRequestHandler<ClearCart, Result>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ClearCart request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(request.UserId);

        if (cart is null)
        {
            return Result.Failure("Корзина не найдена.");
        }

        cart.Items.Clear();
        cart.TotalSum = 0;

        await _cartRepository.UpdateAsync(cart);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}
