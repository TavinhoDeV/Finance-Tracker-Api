using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Domain.Entities;
using MediatR;

namespace FinanceTracker.Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountDto>
{
    private readonly IApplicationDbContext _context;

    public CreateAccountCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = Account.Create(request.Name, request.Type, request.InitialBalance, request.Currency, request.UserId);

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync(cancellationToken);

        return new AccountDto(account.Id, account.Name, account.Type, account.Balance, account.Currency, account.CreatedAt);
    }
}
