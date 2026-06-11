using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
