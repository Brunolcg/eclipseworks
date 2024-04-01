namespace Eclipseworks.Application.Commands.UpdateProjectTask;

public class UpdateProjectTaskCommandValidator : AbstractValidator<UpdateProjectTaskCommand>
{
    public UpdateProjectTaskCommandValidator()
    {
        RuleFor(p => p.ProjectId)
            .NotEmpty()
            .WithMessage(Messages.ProjectIdIsRequired);
        
        RuleFor(p => p.ProjectTaskId)
            .NotEmpty()
            .WithMessage(Messages.ProjectTaskIdIsRequired);
        
        RuleFor(p => p.Status)
            .IsInEnum()
            .WithMessage(Messages.InvalidStatus);
    }
}
