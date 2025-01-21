namespace test.Database.DbService;

public interface IUnitOfWork
{
    Task<int> Commit();
}