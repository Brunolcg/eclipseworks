using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Eclipseworks.Core.Application.Responses;
using Eclipseworks.Core.WebApi.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Qualyteam.Core.WebApi.Controller;

[ApiController]
[Produces("application/json")]
public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;
    private IAuthorizationService? _authorizationService;

    protected IAuthorizationService AuthorizationService 
        => _authorizationService ??= HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();

    public BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected ActionResult CustomResponse<T>(INewResponse<T> response)
        => response.Status switch
        {
            ResponseStatus.Ok when response.Data is null => Ok(),
            ResponseStatus.Ok => Ok(response.Data),
            ResponseStatus.Created => Created(Request.Path.Value ?? string.Empty, response.Data),
            ResponseStatus.Accepted when response is AcceptedResponse<T> accepted => Accepted(accepted.Uri, response.Data),
            ResponseStatus.NoContent => NoContent(),
            ResponseStatus.Forbidden => new ObjectResult(new ApiResponse("Forbidden", Convert.ToInt32(response.Status), response.Messages)) { StatusCode = 403 },
            ResponseStatus.NotFound => NotFound(new ApiResponse("NotFound", Convert.ToInt32(response.Status), response.Messages)),
            ResponseStatus.Unprocessable => UnprocessableEntity(new ApiResponse("BusinessValidation", Convert.ToInt32(response.Status), response.Messages)),
            ResponseStatus.Conflict => Conflict(new ApiResponse("ConflictError", Convert.ToInt32(response.Status), response.Messages)),
            ResponseStatus.Unauthorized => Unauthorized(new ApiResponse("Unauthorized", Convert.ToInt32(response.Status), response.Messages)),
            _ => BadRequest("anErrorOccurred")
        };

    protected async Task<AuthorizationResult> AuthorizeAsync(params string[] permissions)
    {
        var policy = $"permission:{string.Join(',', permissions)}";
        return await AuthorizationService.AuthorizeAsync(User, policy);
    }
}