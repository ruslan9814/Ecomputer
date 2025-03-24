using System.Threading;

namespace Infrasctructure.Cache;

public interface ICacheEntityService
{
    /// <summary>
    /// Получает сущность из кэша по идентификатору
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Найденная сущность или null</returns>
    Task<TEntity?> GetAsync<TEntity>(int id, CancellationToken cancellationToken = default)
        where TEntity : EntityBase;

    /// <summary>
    /// Обновляет срок жизни сущности в кэше
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <param name="entity">Сущность для обновления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task RefreshAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : EntityBase;

    /// <summary>
    /// Сохраняет сущность в кэш
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <param name="entity">Сущность для сохранения</param>
    /// <param name="absoluteExpirationRelativeToNow">Время жизни в кэше</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task SetAsync<TEntity>(
        TEntity entity,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        CancellationToken cancellationToken = default)
        where TEntity : EntityBase;

    /// <summary>
    /// Удаляет сущность из кэша
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync<TEntity>(int id, CancellationToken cancellationToken = default)
        where TEntity : EntityBase;

    /// <summary>
    /// Проверяет наличие сущности в кэше
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>True если сущность есть в кэше</returns>
    Task<bool> ExistsAsync<TEntity>(int id, CancellationToken cancellationToken = default)
        where TEntity : EntityBase;
}