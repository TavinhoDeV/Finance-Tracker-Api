using FinanceTracker.Application.Accounts.Commands.CreateAccount;
using MediatR;

namespace FinanceTracker.Application.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(Guid AccountId, Guid UserId) : IRequest<AccountDto>;
