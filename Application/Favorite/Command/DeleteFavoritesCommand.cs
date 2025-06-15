using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Favorite.Command;

public sealed record DeleteFavoritesCommand(int ProductId) : IRequest<Result>;

internal sealed class DeleteFavoritesCommandHandler(
    IFavoritesRepository favoritesRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteFavoritesCommand, Result>
{
    private readonly IFavoritesRepository _favoritesRepository = favoritesRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteFavoritesCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId <= 0)
        {
            return Result.Failure("Пользователь не авторизован.");
        }
        var favorites = await _favoritesRepository.GetByUserIdAsync(userId);
        if (favorites is null)
        {
            return Result.Failure("Избранное не найдено.");
        }

        if (!favorites.ContainsProduct(request.ProductId))
        {
            return Result.Failure("Продукт не найден в избранном.");
        }

        favorites.RemoveProduct(request.ProductId);
        await _favoritesRepository.UpdateAsync(favorites);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
