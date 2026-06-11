using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    string Description,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime Date,
    Guid AccountId,
    Guid UserId,
    string? Notes = null) : IRequest<TransactionDto>;

public record TransactionDto(
    Guid Id,
    string Description,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime Date,
    string? Notes,
    Guid AccountId,
    DateTime CreatedAt);
