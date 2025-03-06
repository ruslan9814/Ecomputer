using Domain.Users;

namespace Infrasctructure.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetUserByEmailAsync(string email);
    Task<bool> IsEmailExistAsync(string email);
    Task<User> GetUserByConfirmationTokenAsync(string confirmationToken);
}
