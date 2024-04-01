namespace Eclipseworks.Application.Commands.DeleteProject;

public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(p => p.ProjectId)
            .NotEmpty()
            .WithMessage(Messages.ProjectIdIsRequired);
    }
}
