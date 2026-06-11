using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Transaction : BaseEntity
{
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public TransactionCategory Category { get; private set; }
    public DateTime Date { get; private set; }
    public string? Notes { get; private set; }
    public Guid AccountId { get; private set; }

    public Account Account { get; private set; } = null!;

    private Transaction() { }

    public static Transaction Create(
        string description,
        decimal amount,
        TransactionType type,
        TransactionCategory category,
        DateTime date,
        Guid accountId,
        string? notes = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        return new Transaction
        {
            Description = description,
            Amount = amount,
            Type = type,
            Category = category,
            Date = date,
            AccountId = accountId,
            Notes = notes
        };
    }
}
