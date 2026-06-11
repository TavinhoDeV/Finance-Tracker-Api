using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Transactions.Queries.GetMonthlySummary;

public class GetMonthlySummaryQueryHandler : IRequestHandler<GetMonthlySummaryQuery, MonthlySummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetMonthlySummaryQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<MonthlySummaryDto> Handle(GetMonthlySummaryQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == request.AccountId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId);

        if (account.UserId != request.UserId)
            throw new ForbiddenAccessException();

        var transactions = await _context.Transactions
            .Where(t => t.AccountId == request.AccountId
                     && t.Date.Year == request.Year
                     && t.Date.Month == request.Month)
            .ToListAsync(cancellationToken);

        var totalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var totalExpenses = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

        var byCategory = transactions
            .GroupBy(t => t.Category)
            .Select(g => new CategorySummaryDto(g.Key.ToString(), g.Sum(t => t.Amount), g.Count()))
            .OrderByDescending(c => c.Amount)
            .ToList();

        return new MonthlySummaryDto(request.Year, request.Month, totalIncome, totalExpenses, totalIncome - totalExpenses, byCategory);
    }
}
