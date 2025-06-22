using Domain.Users;

namespace Infrasctructure.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Получает пользователя по email
    /// </summary>
    /// <param name="email">Email пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Найденный пользователь или null</returns>
    Task<User?> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверяет существование пользователя с указанным email
    /// </summary>
    /// <param name="email">Email для проверки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>True если пользователь существует</returns>
    Task<bool> IsEmailExistAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает пользователя по токену подтверждения
    /// </summary>
    /// <param name="confirmationToken">Токен подтверждения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Найденный пользователь или null</returns>
    Task<User?> GetUserByConfirmationTokenAsync(
        string confirmationToken,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает пользователя по refresh токену
    /// </summary>
    /// <param name="refreshToken">Refresh токен</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Найденный пользователь или null</returns>
    Task<User?> GetUserByRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Инвалидирует все refresh токены пользователя
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Количество обновленных записей</returns>
    Task<int> InvalidateAllRefreshTokensAsync(
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает пользователя с информацией о роли
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Найденный пользователь или null</returns>
    Task<User?> GetUserWithRoleAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> GetAll(bool includeRelated = false);
    Task UpdateImageUrlAsync(int userId, string imageUrl, CancellationToken cancellationToken = default);
}