using FluentValidation;

namespace FinanceTracker.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.Date).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.AddDays(1));
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Notes).MaximumLength(500).When(x => x.Notes is not null);
    }
}
