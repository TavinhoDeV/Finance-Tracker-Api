using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FluentAssertions;

namespace FinanceTracker.Tests.Domain;

public class AccountEntityTests
{
    [Fact]
    public void Create_ValidData_ShouldCreateAccount()
    {
        var account = Account.Create("Conta Corrente", AccountType.Checking, 1000m, "BRL", Guid.NewGuid());

        account.Should().NotBeNull();
        account.Id.Should().NotBeEmpty();
        account.Name.Should().Be("Conta Corrente");
        account.Balance.Should().Be(1000m);
    }

    [Fact]
    public void ApplyTransaction_Income_IncreasesBalance()
    {
        var account = Account.Create("Test", AccountType.Checking, 1000m, "BRL", Guid.NewGuid());
        account.ApplyTransaction(TransactionType.Income, 500m);
        account.Balance.Should().Be(1500m);
    }

    [Fact]
    public void ApplyTransaction_Expense_DecreasesBalance()
    {
        var account = Account.Create("Test", AccountType.Checking, 1000m, "BRL", Guid.NewGuid());
        account.ApplyTransaction(TransactionType.Expense, 200m);
        account.Balance.Should().Be(800m);
    }

    [Fact]
    public void Create_EmptyName_ThrowsException()
    {
        var act = () => Account.Create("", AccountType.Checking, 0, "BRL", Guid.NewGuid());
        act.Should().Throw<ArgumentException>();
    }
}
