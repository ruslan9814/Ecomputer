namespace Infrasctructure.UnitOfWork;
public interface IUnitOfWork
{
    Task<int> Commit();
}