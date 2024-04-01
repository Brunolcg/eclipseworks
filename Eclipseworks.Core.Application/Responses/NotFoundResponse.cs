namespace Eclipseworks.Core.Application.Responses;

public record NotFoundResponse<T> : NewResponse<T>
{
    public NotFoundResponse(string message)
        : base(ResponseStatus.NotFound, message) { }
}