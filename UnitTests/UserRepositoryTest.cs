using Moq;
using test.Database.Repositories.Interfaces;
using test.Models;

namespace UnitTests;

public class UserRepositoryTest
{
    [Fact]
    public async Task AddAsync()
    {

        var mockUserRepository = new Mock<IUserRepository>();
        var user = new User { Id = 1, Name = "Ruslan" };

        mockUserRepository
            .Setup(repository => repository.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        var userRepository = mockUserRepository.Object;

        await userRepository.AddAsync(user);

        mockUserRepository.Verify(repository => repository.AddAsync(user), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync()
    {
        var mockUserRepository = new Mock<IUserRepository>();

        var user = new User { Id = 1, Name = "Ruslan" };

        mockUserRepository
             .Setup(repository => repository.DeleteAsync(user.Id))
             .Returns(Task.FromResult(true));

        var userRepository = mockUserRepository.Object;

        var result = await userRepository.DeleteAsync(user.Id);

        mockUserRepository.Verify(repository => repository.DeleteAsync(user.Id), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task GetAsync(int id, string name)
    {
        var mockUserRepository = new Mock<IUserRepository>();

        var user = new User { Id = 1, Name = "Ruslan" };

        mockUserRepository.Setup(repository =>
             repository.GetAsync(user.Id))
            .ReturnsAsync(user);

        var userRepository = mockUserRepository.Object;

        var result = await userRepository.GetAsync(user.Id);

        mockUserRepository.Verify(repo => repo.GetAsync(result.Id), Times.Once);
    }
}
