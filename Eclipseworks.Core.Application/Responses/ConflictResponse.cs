namespace Eclipseworks.Core.Application.Responses;

public record ConflictResponse<T> : NewResponse<T>
{
    public ConflictResponse(string message)
        : base(ResponseStatus.Conflict, message) { }
    public ConflictResponse(IEnumerable<string> messages)
        : base(ResponseStatus.Conflict, messages) { }
}
