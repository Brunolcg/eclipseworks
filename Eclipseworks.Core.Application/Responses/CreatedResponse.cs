namespace Eclipseworks.Core.Application.Responses;

public record CreatedResponse<T> : NewResponse<T>
{
    public CreatedResponse()
        : base(ResponseStatus.Created) { }
    public CreatedResponse(T data)
        : base(ResponseStatus.Created, data) { }
}