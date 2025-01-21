using Microsoft.AspNetCore.Mvc;
using Test.Database.Repositories.Interfaces;
using Test.Models;
using Test.Services.UserService;
using Test.Endpoints.Users.Requests;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<int> AddUser([FromBody] RegistUserRequest userRequest)
    {
        if (userRequest is null || string.IsNullOrEmpty(userRequest.Username) || string.IsNullOrEmpty(userRequest.Email) || string.IsNullOrEmpty(userRequest.Password))
        {
            throw new ArgumentException("Invalid user request data.");
        }

        var existingUser = await _userRepository.GetUserByEmailAsync(userRequest.Email);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var hashedPassword = HashPassword(userRequest.Password);

        var user = new User(userRequest.Username, userRequest.Email, hashedPassword);
        await _userRepository.AddAsync(user);
        return user.Id;
    }


    public async Task<bool> UserExistsAsync(int id)
    {
        if (id <= 0)
        {
            return false;
        }
        var user = await _userRepository.GetAsync(id);
        return user != null;
    }

    public async Task<User> GetUser(GetUserRequest userRequest)
    {
        if (userRequest.Id <= 0)
        {
            throw new ArgumentException("Invalid user ID.");
        }

        var user = await _userRepository.GetAsync(userRequest.Id);
        return user ?? throw new KeyNotFoundException("User not found.");
    }

    public async Task<bool> UpdateUser([FromBody] UpdateUserRequest userRequest)
    {
        if (userRequest is null || userRequest.Id <= 0 || string.IsNullOrEmpty(userRequest.Username))
        {
            throw new ArgumentException("Invalid user entity data.");
        }

        var existingUser = await _userRepository.GetAsync(userRequest.Id) ?? throw new InvalidOperationException("User not found.");
        existingUser.Name = userRequest.Username;
        await _userRepository.UpdateAsync(existingUser);
        return true;
    }


    public async Task<bool> DeleteUser([FromBody] RemoveUserRequest removeUserRequest)
    {
        if (removeUserRequest is null || removeUserRequest.Id <= 0)
        {
            throw new ArgumentException("Invalid user ID.");
        }

        var user = await _userRepository.GetAsync(removeUserRequest.Id) ?? throw new InvalidOperationException("User not found.");
        await _userRepository.DeleteAsync(removeUserRequest.Id);
        return true;
    }


    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
    }


    public async Task<User> LoginUser(LoginUserRequest loginUserRequest)
    {
        if (loginUserRequest is null || string.IsNullOrEmpty(loginUserRequest.Email) || string.IsNullOrEmpty(loginUserRequest.Password))
        {
            throw new ArgumentException("Invalid login request data.");
        }

        var user = await _userRepository.GetUserByEmailAsync(loginUserRequest.Email) ?? throw new InvalidOperationException("User not found.");
        bool passwordIsValid = VerifyPassword(loginUserRequest.Password, user.HashedPassword);

        if (!passwordIsValid)
        {
            throw new InvalidOperationException("Invalid password.");
        }

        return user;
    }


}
