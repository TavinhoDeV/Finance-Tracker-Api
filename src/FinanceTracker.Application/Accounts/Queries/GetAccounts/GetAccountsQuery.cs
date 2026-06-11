using FinanceTracker.Application.Accounts.Commands.CreateAccount;
using MediatR;

namespace FinanceTracker.Application.Accounts.Queries.GetAccounts;

public record GetAccountsQuery(Guid UserId) : IRequest<List<AccountDto>>;
