namespace Presentation.Users.Requests;

public sealed record UpdateRefreshTokenRequest(string Token, string RefreshToken);
