using MediatR;

namespace FinanceTracker.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

public record LoginResponse(Guid UserId, string Name, string Email, string Token);
