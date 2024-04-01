namespace Eclipseworks.Application.Commands.CreateProjectTask;

public class CreateProjectTaskCommandValidator : AbstractValidator<CreateProjectTaskCommand>
{
    public CreateProjectTaskCommandValidator()
    {
        RuleFor(p => p.ProjectId)
            .NotEmpty()
            .WithMessage(Messages.ProjectIdIsRequired);
        
        RuleFor(p => p.Priority)
            .IsInEnum()
            .WithMessage(Messages.InvalidPriority);
    }
}
