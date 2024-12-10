using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using test.Database.Repositories.Interfaces;
using test.Models;

namespace test.Database.Repositories.Classes;

public class UserRepository(ApplicationDbContext dbContext, IDistributedCache cache) :
    BaseRepository<User>(dbContext, cache), IUserRepository
{

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _dbContext.Users
            .AnyAsync(u => u.Id == userId);
    }

}
