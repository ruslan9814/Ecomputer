using Microsoft.EntityFrameworkCore;
using Domain.Users;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;

namespace Infrasctructure.Repositories.Classes;

public class UserRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<User>(dbContext, cache), IUserRepository
{
    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await
            _dbContext.Set<User>().Where(x => x.Email == email).FirstAsync();

        await _cache.SetAsync(user);

        return user;
    }

    public async Task<bool> IsEmailExistAsync(string email)
    {
        return await _dbContext.Set<User>().AnyAsync(x => x.Email == email);
    }

    public async Task<User> GetUserByConfirmationTokenAsync(string token) =>
        await _dbContext.Set<User>().FirstAsync(u => u.ConfirmationToken == token);
}
