namespace Eclipseworks.Core.Domain.Result;

public abstract record Result
{
    public static Result Success() => new SuccessResult();

    public static Result<TValue> Success<TValue>(TValue value) => new SuccessResult<TValue>(value: value);

    public static Result Failure(string error) => new FailureResult(error: error);

    public static Result<TValue> Failure<TValue>(string error) => new FailureResult<TValue>(error: error);
}

public abstract record Result<TValue>
{
    public TValue? Value { get; }

    protected internal Result(TValue? value) => Value = value;
}

public record SuccessResult : Result
{
    protected internal SuccessResult()
    {
    }
}

public record SuccessResult<TValue> : Result<TValue>
{
    protected internal SuccessResult(TValue? value) : base(value)
    {
    }
}

public record FailureResult : Result
{
    public string Error { get; }

    protected internal FailureResult(string error)
    {
        Error = error;
    }
}

public record FailureResult<TValue> : Result<TValue>
{
    public string Error { get; }

    protected internal FailureResult(string error) : base(value: default)
    {
        Error = error;
    }

    public static implicit operator Result(FailureResult<TValue> failureResult) => Result.Failure(failureResult.Error);
}