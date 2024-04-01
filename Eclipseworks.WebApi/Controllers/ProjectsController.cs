namespace Qualyteam.Opportunities.WebApi.Controllers;

[Route("api/projects")]
public class ProjectsController : BaseController
{
    public ProjectsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ListProjectsViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListProjectsViewModel>>> ListProjects()
        => CustomResponse(await _mediator.Send(new ListProjectsQuery()));
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Unit>> CreateProject([FromBody] CreateProjectCommand command)
         => CustomResponse(await _mediator.Send(command));
    
    [HttpPost("{projectId:guid}/tasks")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Guid>> CreateProjectTask(Guid projectId, [FromBody] CreateProjectTaskPayload payload)
        => CustomResponse(await _mediator.Send(payload.AsCommand(projectId)));
    
    [HttpGet("{projectId:guid}/tasks")]
    [ProducesResponseType(typeof(IEnumerable<ListProjectsViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListProjectsViewModel>>> ListProjectTasks(Guid projectId)
        => CustomResponse(await _mediator.Send(new ListProjectTasksQuery(projectId)));
    
    [HttpPut("{projectId:guid}/tasks/{projectTaskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Unit>> UpdateProjectTask(Guid projectId, Guid projectTaskId, [FromBody] UpdateProjectTaskPayload payload)
        => CustomResponse(await _mediator.Send(payload.AsCommand(projectId, projectTaskId)));
    
    [HttpDelete("{projectId:guid}/tasks/{projectTaskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Unit>> DeleteProjectTask(Guid projectId, Guid projectTaskId)
        => CustomResponse(await _mediator.Send(new DeleteProjectTaskCommand(projectId, projectTaskId)));
    
    [HttpDelete("{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Unit>> DeleteProject(Guid projectId)
        => CustomResponse(await _mediator.Send(new DeleteProjectCommand(projectId)));
    
    [HttpPost("{projectId:guid}/tasks/{projectTaskId:guid}/comments")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Unit>> CreateProjectTaskComment(Guid projectId, Guid projectTaskId, [FromBody] CreateProjectTaskCommentPayload payload)
        => CustomResponse(await _mediator.Send(payload.AsCommand(projectId, projectTaskId)));
}