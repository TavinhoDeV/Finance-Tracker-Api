using MediatR;

namespace FinanceTracker.Application.Transactions.Commands.DeleteTransaction;

public record DeleteTransactionCommand(Guid TransactionId, Guid UserId) : IRequest;
