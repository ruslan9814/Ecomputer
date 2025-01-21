namespace Test.Models.Core;

public abstract class EntityBase
{
    public int Id { get; protected set; }

    protected EntityBase(int id)
    {
        Id = id;
    }

    protected EntityBase()
    {
    }
}
