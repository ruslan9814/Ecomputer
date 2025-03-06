namespace Domain.Core;

public class Result
{
    public string? Error { get; private set; }

    public bool IsSuccess => Error == null;

    public bool IsFailure => !IsSuccess;

    public Result() { }

    public Result(string error) =>
        Error = error;

    public static Result Success() => new();
    public static Result Failure(string message) => new(message);

    public static Result<T> Failure<T>(string message) => new(message);
    public static Result<T> Success<T>(T value) => new(value);
}

public class Result<T> : Result
{
    public T Value { get; private set; } = default!;

    public Result() { }

    public Result(string error) : base(error) { }

    public Result(T value)
    {
        Value = value;
    }
}