namespace Domain.Categories;

public class Category : EntityBase
{
    public string Name { get; set; } = null!;

    private Category()
    {
    }

    public Category(int id, string name) : base(id)
    {
        Name = name;
    }

    public Category(string name)
    {
        Name = name;
    }

}