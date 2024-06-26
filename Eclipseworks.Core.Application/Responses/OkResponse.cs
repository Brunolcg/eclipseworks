﻿namespace Eclipseworks.Core.Application.Responses;

public record OkResponse<T> : NewResponse<T>
{
    public OkResponse()
        : base(ResponseStatus.Ok) { }
    public OkResponse(T data)
        : base(ResponseStatus.Ok, data) { }
}