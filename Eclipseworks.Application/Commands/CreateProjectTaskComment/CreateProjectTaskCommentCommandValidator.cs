namespace Eclipseworks.Application.Commands.CreateProjectTaskComment;

public class CreateProjectTaskCommentCommandValidator : AbstractValidator<CreateProjectTaskCommentCommand>
{
    public CreateProjectTaskCommentCommandValidator()
    {
        RuleFor(p => p.ProjectId)
            .NotEmpty()
            .WithMessage(Messages.ProjectIdIsRequired);
        
        RuleFor(p => p.ProjectTaskId)
            .NotEmpty()
            .WithMessage(Messages.ProjectTaskIdIsRequired);
        
        RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage(Messages.DescriptionIsRequired);
    }
}
