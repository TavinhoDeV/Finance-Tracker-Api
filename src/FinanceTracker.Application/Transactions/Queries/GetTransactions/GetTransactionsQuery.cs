using FinanceTracker.Application.Transactions.Commands.CreateTransaction;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery(
    Guid AccountId,
    Guid UserId,
    DateTime? From = null,
    DateTime? To = null,
    TransactionType? Type = null,
    TransactionCategory? Category = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedTransactionsDto>;

public record PagedTransactionsDto(
    List<TransactionDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
