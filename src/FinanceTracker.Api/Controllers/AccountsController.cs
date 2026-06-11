using FinanceTracker.Application.Accounts.Commands.CreateAccount;
using FinanceTracker.Application.Accounts.Commands.DeleteAccount;
using FinanceTracker.Application.Accounts.Queries.GetAccountById;
using FinanceTracker.Application.Accounts.Queries.GetAccounts;
using FinanceTracker.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all accounts for the authenticated user</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<AccountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAccountsQuery(User.GetUserId()), ct);
        return Ok(result);
    }

    /// <summary>Get account by ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAccountByIdQuery(id, User.GetUserId()), ct);
        return Ok(result);
    }

    /// <summary>Create a new account</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request, CancellationToken ct)
    {
        var command = new CreateAccountCommand(
            request.Name, request.Type, request.InitialBalance, request.Currency, User.GetUserId());
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Delete an account</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAccountCommand(id, User.GetUserId()), ct);
        return NoContent();
    }
}

public record CreateAccountRequest(
    string Name,
    Domain.Enums.AccountType Type,
    decimal InitialBalance,
    string Currency = "BRL");
