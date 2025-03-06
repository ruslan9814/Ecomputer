using Infrasctructure.Database;

namespace Infrasctructure.UnitOfWork;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int> Commit() =>
        await _context.SaveChangesAsync();
}
