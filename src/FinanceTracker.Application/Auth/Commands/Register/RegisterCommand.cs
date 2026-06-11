using MediatR;

namespace FinanceTracker.Application.Auth.Commands.Register;

public record RegisterCommand(string Name, string Email, string Password) : IRequest<RegisterResponse>;

public record RegisterResponse(Guid UserId, string Name, string Email, string Token);
