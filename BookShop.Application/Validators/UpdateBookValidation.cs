using BookShop.Application.Commands;
using FluentValidation;

namespace BookShop.Application.Validators;

public class UpdateBookValidation: AbstractValidator<UpdateBookCommand>
{
    public UpdateBookValidation()
    {
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Max length is 100")
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Max length is 500");

        RuleFor(x => x.ReleaseDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Cant use future date");
    }
}