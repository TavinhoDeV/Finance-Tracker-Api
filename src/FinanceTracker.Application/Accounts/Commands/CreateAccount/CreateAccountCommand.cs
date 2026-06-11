using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(
    string Name,
    AccountType Type,
    decimal InitialBalance,
    string Currency,
    Guid UserId) : IRequest<AccountDto>;

public record AccountDto(Guid Id, string Name, AccountType Type, decimal Balance, string Currency, DateTime CreatedAt);
