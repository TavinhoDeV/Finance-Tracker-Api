using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Account : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public AccountType Type { get; private set; }
    public decimal Balance { get; private set; }
    public string Currency { get; private set; } = "BRL";
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    private Account() { }

    public static Account Create(string name, AccountType type, decimal initialBalance, string currency, Guid userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Account
        {
            Name = name,
            Type = type,
            Balance = initialBalance,
            Currency = currency,
            UserId = userId
        };
    }

    public void ApplyTransaction(TransactionType transactionType, decimal amount)
    {
        Balance = transactionType == TransactionType.Income
            ? Balance + amount
            : Balance - amount;

        SetUpdatedAt();
    }

    public void Update(string name, AccountType type, string currency)
    {
        Name = name;
        Type = type;
        Currency = currency;
        SetUpdatedAt();
    }
}
