using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Application.Transactions.Commands.CreateTransaction;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Transactions.Queries.GetTransactions;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, PagedTransactionsDto>
{
    private readonly IApplicationDbContext _context;

    public GetTransactionsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<PagedTransactionsDto> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == request.AccountId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId);

        if (account.UserId != request.UserId)
            throw new ForbiddenAccessException();

        var query = _context.Transactions
            .Where(t => t.AccountId == request.AccountId);

        if (request.From.HasValue)
            query = query.Where(t => t.Date >= request.From.Value);
        if (request.To.HasValue)
            query = query.Where(t => t.Date <= request.To.Value);
        if (request.Type.HasValue)
            query = query.Where(t => t.Type == request.Type.Value);
        if (request.Category.HasValue)
            query = query.Where(t => t.Category == request.Category.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(t => t.Date)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new TransactionDto(t.Id, t.Description, t.Amount, t.Type, t.Category, t.Date, t.Notes, t.AccountId, t.CreatedAt))
            .ToListAsync(cancellationToken);

        return new PagedTransactionsDto(items, totalCount, request.Page, request.PageSize);
    }
}
