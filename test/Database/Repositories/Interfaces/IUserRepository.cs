using test.Models;

namespace test.Database.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> UserExistsAsync(int id);
}
