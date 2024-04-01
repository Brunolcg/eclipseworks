using FluentValidation.Results;

namespace Eclipseworks.Core.Application.Responses;

public record UnprocessableResponse<T> : NewResponse<T>
{
    public UnprocessableResponse(string message)
        : base(ResponseStatus.Unprocessable, message) { }
    public UnprocessableResponse(IEnumerable<string> messages)
        : base(ResponseStatus.Unprocessable, messages) { }
    public UnprocessableResponse(ValidationResult validate)
        : base(ResponseStatus.Unprocessable, validate.Errors.Select(x => x.ErrorMessage).ToList()) { }
}