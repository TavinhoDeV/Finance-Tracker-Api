using FinanceTracker.Application.Transactions.Commands.CreateTransaction;
using FinanceTracker.Application.Transactions.Commands.DeleteTransaction;
using FinanceTracker.Application.Transactions.Queries.GetMonthlySummary;
using FinanceTracker.Application.Transactions.Queries.GetTransactions;
using FinanceTracker.Api.Extensions;
using FinanceTracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/accounts/{accountId:guid}/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get transactions with optional filters and pagination</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedTransactionsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        Guid accountId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] TransactionType? type,
        [FromQuery] TransactionCategory? category,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetTransactionsQuery(accountId, User.GetUserId(), from, to, type, category, page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Get monthly summary for an account</summary>
    [HttpGet("summary/{year:int}/{month:int}")]
    [ProducesResponseType(typeof(MonthlySummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMonthlySummary(Guid accountId, int year, int month, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMonthlySummaryQuery(accountId, User.GetUserId(), year, month), ct);
        return Ok(result);
    }

    /// <summary>Create a new transaction</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(Guid accountId, [FromBody] CreateTransactionRequest request, CancellationToken ct)
    {
        var command = new CreateTransactionCommand(
            request.Description, request.Amount, request.Type,
            request.Category, request.Date, accountId, User.GetUserId(), request.Notes);
        var result = await _mediator.Send(command, ct);
        return Created(string.Empty, result);
    }

    /// <summary>Delete a transaction</summary>
    [HttpDelete("{transactionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid accountId, Guid transactionId, CancellationToken ct)
    {
        await _mediator.Send(new DeleteTransactionCommand(transactionId, User.GetUserId()), ct);
        return NoContent();
    }
}

public record CreateTransactionRequest(
    string Description,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime Date,
    string? Notes = null);
