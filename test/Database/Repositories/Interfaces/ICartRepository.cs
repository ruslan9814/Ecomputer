using test.Models;

namespace test.Database.Repositories.Interfaces;

public interface ICartRepository : IBaseRepository<Cart>
{
    Task<bool> CartExistsAsync(int cartId);
    Task<Cart> GetAsync(int cartId);
}
