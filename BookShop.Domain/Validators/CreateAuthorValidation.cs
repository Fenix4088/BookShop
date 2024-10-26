using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace BookShop.Domain.Validators;

public class CreateAuthorValidation: AbstractValidator<AuthorEntity>
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