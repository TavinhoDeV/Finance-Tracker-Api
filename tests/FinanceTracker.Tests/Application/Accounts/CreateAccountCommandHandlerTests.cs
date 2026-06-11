using FinanceTracker.Application.Accounts.Commands.CreateAccount;
using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FinanceTracker.Tests.Application.Accounts;

public class CreateAccountCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly CreateAccountCommandHandler _handler;

    public CreateAccountCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new CreateAccountCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsAccountDto()
    {
        // Arrange
        var accounts = new List<Account>();
        var mockSet = CreateMockDbSet(accounts);
        _contextMock.Setup(c => c.Accounts).Returns(mockSet.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateAccountCommand("Conta Corrente", AccountType.Checking, 1000m, "BRL", Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Conta Corrente");
        result.Balance.Should().Be(1000m);
        result.Currency.Should().Be("BRL");
        result.Type.Should().Be(AccountType.Checking);
    }

    [Fact]
    public async Task Handle_SavesAccountToDatabase()
    {
        // Arrange
        var accounts = new List<Account>();
        var mockSet = CreateMockDbSet(accounts);
        _contextMock.Setup(c => c.Accounts).Returns(mockSet.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateAccountCommand("Poupança", AccountType.Savings, 500m, "BRL", Guid.NewGuid());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.Add);
        return mockSet;
    }
}
