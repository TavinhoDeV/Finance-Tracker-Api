using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly IApplicationDbContext _context;

    public CreateTransactionCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == request.AccountId, cancellationToken)
            ?? throw new NotFoundException(nameof(Account), request.AccountId);

        if (account.UserId != request.UserId)
            throw new ForbiddenAccessException();

        var transaction = Transaction.Create(
            request.Description,
            request.Amount,
            request.Type,
            request.Category,
            request.Date,
            request.AccountId,
            request.Notes);

        account.ApplyTransaction(request.Type, request.Amount);

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);

        return new TransactionDto(
            transaction.Id,
            transaction.Description,
            transaction.Amount,
            transaction.Type,
            transaction.Category,
            transaction.Date,
            transaction.Notes,
            transaction.AccountId,
            transaction.CreatedAt);
    }
}
