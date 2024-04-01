namespace Eclipseworks.Core.Application.Responses;

public record NoContentResponse<T> : NewResponse<T>
{
    public NoContentResponse()
        : base(ResponseStatus.NoContent) { }
}