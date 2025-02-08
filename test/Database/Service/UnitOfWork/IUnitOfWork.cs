namespace test.Database.Service.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> Commit();
}