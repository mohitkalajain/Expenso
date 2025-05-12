using FluentValidation;
using MonthlyExpenseTracker.DTO;

namespace MonthlyExpenseTracker.Helper.Validator;

public class CategoryValidator: AbstractValidator<CategoryDTO>
{
    public CategoryValidator()
    {
        RuleFor(e => e.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(20).WithMessage("Category name can't be longer than 20 characters.");
    }
}
public class CategoryDeleteValidator : AbstractValidator<int>
{
    public CategoryDeleteValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Category ID is required.")
            .GreaterThan(0).WithMessage("Category ID must be greater than zero.");
    }
}
