using Test.Database.Repositories.Interfaces;
using Test.Endpoints.Users.Requests;
using Test.Models;

namespace Test.Services.UserService;

public interface IUserService
{
    Task<int> AddUser(RegistUserRequest userRequest); 
    Task<bool> UserExistsAsync(int id);
    Task<User> GetUser(GetUserRequest getUserRequest);
    Task<bool> UpdateUser(UpdateUserRequest userRequest); 
    Task<bool> DeleteUser(RemoveUserRequest removeUserRequest);
    Task<User> LoginUser(LoginUserRequest loginUserRequest);
}
