namespace Eclipseworks.Application.Commands.DeleteProjectTask;

public class DeleteProjectTaskCommandValidator : AbstractValidator<DeleteProjectTaskCommand>
{
    public DeleteProjectTaskCommandValidator()
    {
        RuleFor(p => p.ProjectId)
            .NotEmpty()
            .WithMessage(Messages.ProjectIdIsRequired);
        
        RuleFor(p => p.ProjectTaskId)
            .NotEmpty()
            .WithMessage(Messages.ProjectTaskIdIsRequired);
    }
}
