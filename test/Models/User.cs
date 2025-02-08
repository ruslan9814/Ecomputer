using Microsoft.EntityFrameworkCore;
using test.Common;
using Test.Infrastrcture.Jwt;
using Test.Models.Core;

namespace Test.Models;

public class User : EntityBase
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public Cart? Cart { get; set; }
    public Role Role { get; set; }

    public User()
    {
        
    }
    public User(string name, Cart cart)
    {
        Name = name;
        Cart = cart;
    }

    public User(string name, string email, string hashedPassword)
    {
        Name = name;
        Email = email;
        HashedPassword = hashedPassword;
    }

    public User(int id, string name, string email, string hashedPassword) : base(id) 
    {
        Id = id;    
        Name = name;
        Email = email;
        HashedPassword = hashedPassword;
    }

    public  Result Login(bool isVerify)
    {
        if (!isVerify)
        {
            return Result.Failure("Пользователь с таким email не найден.");
        }

        return Result.Success;
    }
}