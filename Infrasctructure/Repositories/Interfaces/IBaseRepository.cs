using Domain.Core;

namespace Infrasctructure.Repositories.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : EntityBase
{
    Task<bool> IsExistAsync(int id);
    Task<TEntity> GetAsync(int id);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(int id);
}
