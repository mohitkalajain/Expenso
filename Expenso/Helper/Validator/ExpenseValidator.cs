using FluentValidation;
using MonthlyExpenseTracker.DTO;

namespace MonthlyExpenseTracker.Helper.Validator
{
    public class ExpenseValidator : AbstractValidator<ExpenseDTO>
    {
        public ExpenseValidator()
        {


            RuleFor(e => e.Note)
                .MaximumLength(500).WithMessage("Note can't be longer than 500 characters.");

          

            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.PaymentMethod).NotEmpty().WithMessage("Payment method is required.");
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Valid category is required.");
            RuleFor(x=>x.Note).NotEmpty().WithMessage("Note is required.");
            RuleFor(x => x.Note)
                                .NotEmpty()
                                .WithMessage("Note is required.")
                                .MaximumLength(500)
                                .WithMessage("Note can't exceed 500 characters.");

        }
    }


    public class ExpenseDeleteValidator : AbstractValidator<int>
    {
        public ExpenseDeleteValidator()
        {
            RuleFor(x => x)
            .NotEmpty().WithMessage("Expense ID is required.")
            .GreaterThan(0).WithMessage("Expense ID must be greater than zero.");
        }
    }
}
