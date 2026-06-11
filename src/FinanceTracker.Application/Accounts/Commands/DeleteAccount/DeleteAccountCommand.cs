using MediatR;

namespace FinanceTracker.Application.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid AccountId, Guid UserId) : IRequest;
