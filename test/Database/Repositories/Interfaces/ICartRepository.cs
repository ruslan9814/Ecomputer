using Test.Models;

namespace Test.Database.Repositories.Interfaces;

public interface ICartRepository : IBaseRepository<Cart>
{
    Task<Cart> GetByUserIdAsync(int id);
    Task<bool> IsExistByUserIdAsync(int id);
}
