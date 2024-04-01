namespace Eclipseworks.Core.Application.Responses;

public record AcceptedResponse<T> : NewResponse<T>
{
    public string? Uri { get; init; }

    public AcceptedResponse()
        : base(ResponseStatus.Accepted)
    {
        Uri = null;
    }

    public AcceptedResponse(string uri)
        : base(ResponseStatus.Accepted)
    {
        Uri = uri;
    }

    public AcceptedResponse(T data)
        : base(ResponseStatus.Accepted, data)
    {
        Uri = null;
    }

    public AcceptedResponse(string uri, T data)
        : base(ResponseStatus.Accepted, data)
    {
        Uri = uri;
    }
}
