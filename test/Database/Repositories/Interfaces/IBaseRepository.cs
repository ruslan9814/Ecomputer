namespace test.Database.Repositories.Interfaces;

public interface IBaseRepository<TEntity>
{
    public Task<TEntity> GetAsync(int id);
    public Task AddAsync(TEntity entity);
    public Task UpdateAsync(TEntity entity);
    public Task<bool> DeleteAsync(int id);
}
