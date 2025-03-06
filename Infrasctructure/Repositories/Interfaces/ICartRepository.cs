using Domain.Carts;

namespace Infrasctructure.Repositories.Interfaces;

public interface ICartRepository : IBaseRepository<Cart>
{
    Task<Cart> GetByUserIdAsync(int id);
    Task<bool> IsExistByUserIdAsync(int id);
}
