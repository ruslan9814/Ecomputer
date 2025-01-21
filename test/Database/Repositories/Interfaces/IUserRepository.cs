using Test.Models;

namespace Test.Database.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetUserByEmailAsync(string email);
}
