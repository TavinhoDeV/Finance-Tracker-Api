using FinanceTracker.Application.Accounts.Commands.CreateAccount;
using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
{
    private readonly IApplicationDbContext _context;

    public GetAccountByIdQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == request.AccountId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId);

        if (account.UserId != request.UserId)
            throw new ForbiddenAccessException();

        return new AccountDto(account.Id, account.Name, account.Type, account.Balance, account.Currency, account.CreatedAt);
    }
}
