using test.Common;
using Test.Database.Repositories.Interfaces;
using Test.Models;

namespace test.CQRS.Carts.Commands;

public sealed record AddCart(int UserId, ICollection<CartItem> Items) : IRequest<Result>;

public sealed class AddCartHandler(ICartRepository cartRepository, IUserRepository userRepository) : IRequestHandler<AddCart, Result>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(AddCart request, CancellationToken cancellationToken)
    {
        var isUserExists = await _userRepository.IsExistAsync(request.UserId);

        if (!isUserExists)
        {
            return Result.Failure("Юзер не существует.");
        }

        var existingCart = await _cartRepository.GetAsync(request.UserId);

        if (existingCart != null)
        {
            foreach (var item in request.Items)
            {
                existingCart.Items.Add(item);
            }

            existingCart.TotalSum = existingCart.Items.Sum(x => x.Product.Price * x.Quantity);

            await _cartRepository.UpdateAsync(existingCart);
            return Result.Success;
        }

        return Result.Success;
    }
}


