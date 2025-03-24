using Microsoft.EntityFrameworkCore;
using Domain.Users;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;

namespace Infrasctructure.Repositories.Classes;

public class UserRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<User>(dbContext, cache), IUserRepository
{
    public async Task<User?> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user is not null && _cache is not null)
        {
            await _cache.SetAsync(user, cancellationToken: cancellationToken);
        }

        return user;
    }

    public async Task<bool> IsEmailExistAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetUserByConfirmationTokenAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.ConfirmationToken == token, cancellationToken);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(
     string hashedToken,
     CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == hashedToken, cancellationToken);
    }

    public async Task<int> InvalidateAllRefreshTokensAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([userId], cancellationToken);
        if (user is null) return 0;

        user.RefreshToken = null!;
        user.RefreshTokenExpiryTime = DateTime.UtcNow;
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetUserWithRoleAsync(int userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Update(user);
        await _cache.SetAsync(user, cancellationToken: cancellationToken);
    }
}
