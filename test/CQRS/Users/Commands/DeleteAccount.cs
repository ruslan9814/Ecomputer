using test.Common;

public sealed record DeleteAccount(int UserId) : IRequest<Result>;
