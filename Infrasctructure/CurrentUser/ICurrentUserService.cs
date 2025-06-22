namespace Infrasctructure.CurrentUser;

public interface ICurrentUserService
{
    int UserId { get; }
    bool IsAuthenticated { get; }
}