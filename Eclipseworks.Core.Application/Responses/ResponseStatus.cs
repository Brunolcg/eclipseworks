namespace Eclipseworks.Core.Application.Responses;

public enum ResponseStatus
{
    Ok = 200,
    Created = 201,
    Accepted = 202,
    NoContent = 204,
    File = 220,
    BadRequest = 400,
    Unauthorized = 401,
    PaymentRequired = 402,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    Unprocessable = 422,
    Undefined = 999
}