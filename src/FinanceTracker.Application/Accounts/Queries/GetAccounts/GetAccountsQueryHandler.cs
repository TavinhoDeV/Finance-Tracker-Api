using FinanceTracker.Application.Accounts.Commands.CreateAccount;
using FinanceTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Accounts.Queries.GetAccounts;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, List<AccountDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAccountsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Accounts
            .Where(a => a.UserId == request.UserId)
            .OrderBy(a => a.Name)
            .Select(a => new AccountDto(a.Id, a.Name, a.Type, a.Balance, a.Currency, a.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
