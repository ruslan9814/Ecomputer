using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using test.Common;
using Test.Cache;
using Test.Database.Repositories.Interfaces;
using Test.Infrastrcture.Jwt;
using Test.Models;

namespace Test.Database.Repositories.Classes;

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
}
