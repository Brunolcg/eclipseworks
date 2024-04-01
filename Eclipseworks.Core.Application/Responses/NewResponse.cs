namespace Eclipseworks.Core.Application.Responses;

public interface INewResponse
{
    ResponseStatus Status { get; }
    ISet<string> Messages { get; }
    bool IsSuccess { get; }
    bool IsFailure { get; }
}
    
public interface INewResponse<out T> : INewResponse
{
    T? Data { get; }
}
    
public abstract record NewResponse<T> : INewResponse<T>
{
    public ResponseStatus Status { get; init; } = ResponseStatus.Undefined;
    public T? Data { get; init; }
    public ISet<string> Messages { get; init; } = new HashSet<string>();

    public bool IsSuccess => Messages.Count == 0;
    public bool IsFailure => !IsSuccess;

    protected NewResponse(ResponseStatus status) => Status = status;
    protected NewResponse(ResponseStatus status, T data)
    {
        Status = status;
        Data = data;
    }
    protected NewResponse(ResponseStatus status, string message)
    {
        Status = status;
        Messages.Add(message);
    }

    protected NewResponse(ResponseStatus status, IEnumerable<string> messages)
    {
        Status = status;
        Messages.UnionWith(messages);
    }
}