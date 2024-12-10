namespace test.Models;

public class User
{
    public User(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public User(int id, string name, Cart cart)
    {
        Id = id;
        Name = name;
        Cart = cart;
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Cart? Cart { get; set; }
    public Role Role { get; set; }
}

