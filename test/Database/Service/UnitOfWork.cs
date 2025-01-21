using Npgsql.Replication.PgOutput.Messages;
using Test.Database;

namespace test.Database.DbService;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int> Commit() =>
        await _context.SaveChangesAsync();
}
