namespace test.Common;

public class Result
{
    public string? Error { get; private set; }

    public bool IsSuccess => Error == null;

    public bool IsFailure => !IsSuccess;

    public Result() { }

    public Result(string error) =>
        Error = error;

    public static Result Success => new();
    public static Result Failure(string message) => new(message);
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
    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string message) => new(message);
}
