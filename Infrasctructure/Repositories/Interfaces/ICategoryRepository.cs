using Domain.Categories;

namespace Infrasctructure.Repositories.Interfaces;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<bool> IsExistByNameAsync(string name);
};
