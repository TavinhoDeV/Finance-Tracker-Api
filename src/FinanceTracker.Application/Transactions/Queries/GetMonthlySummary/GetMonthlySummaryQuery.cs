using MediatR;

namespace FinanceTracker.Application.Transactions.Queries.GetMonthlySummary;

public record GetMonthlySummaryQuery(Guid AccountId, Guid UserId, int Year, int Month) : IRequest<MonthlySummaryDto>;

public record MonthlySummaryDto(
    int Year,
    int Month,
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal NetBalance,
    List<CategorySummaryDto> ByCategory);

public record CategorySummaryDto(string Category, decimal Amount, int Count);
