using BookShop.Application.Commands;
using FluentValidation;

namespace BookShop.Application.Validators;

public class CreateAuthorValidation: AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorValidation()
    {
        RuleFor(author => author.Name)
            .NotEmpty()
            .WithMessage("Please Specified Author Name")
            .MaximumLength(100)
            .WithMessage("Name max length: 100");
        
        RuleFor(author => author.Surname)
            .NotEmpty()
            .WithMessage("Please Specified Author Surname")
            .MaximumLength(100)
            .WithMessage("Surname max length: 100");
    }

}