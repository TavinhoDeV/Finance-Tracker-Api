using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Application.Transactions.Commands.CreateTransaction;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FinanceTracker.Tests.Application.Transactions;

public class CreateTransactionCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly CreateTransactionCommandHandler _handler;

    public CreateTransactionCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new CreateTransactionCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_AccountNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var accounts = new List<Account>();
        var mockAccounts = CreateMockDbSet(accounts);
        _contextMock.Setup(c => c.Accounts).Returns(mockAccounts.Object);

        var command = new CreateTransactionCommand(
            "Salário", 5000m, TransactionType.Income,
            TransactionCategory.Salary, DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DifferentUser_ThrowsForbiddenAccessException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var account = Account.Create("Test", AccountType.Checking, 0, "BRL", userId);

        var accounts = new List<Account> { account };
        var mockAccounts = CreateMockDbSet(accounts);
        _contextMock.Setup(c => c.Accounts).Returns(mockAccounts.Object);

        var command = new CreateTransactionCommand(
            "Test", 100m, TransactionType.Expense,
            TransactionCategory.Food, DateTime.UtcNow, account.Id, Guid.NewGuid()); // different userId

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
       
        return mockSet;
    }
}
