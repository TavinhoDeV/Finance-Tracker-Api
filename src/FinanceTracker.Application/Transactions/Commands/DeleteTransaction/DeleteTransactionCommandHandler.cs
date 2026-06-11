using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTransactionCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _context.Transactions
            .Include(t => t.Account)
            .FirstOrDefaultAsync(t => t.Id == request.TransactionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Transaction), request.TransactionId);

        if (transaction.Account.UserId != request.UserId)
            throw new ForbiddenAccessException();

        // Reverse balance effect
        var reverseType = transaction.Type == Domain.Enums.TransactionType.Income
            ? Domain.Enums.TransactionType.Expense
            : Domain.Enums.TransactionType.Income;

        transaction.Account.ApplyTransaction(reverseType, transaction.Amount);

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
