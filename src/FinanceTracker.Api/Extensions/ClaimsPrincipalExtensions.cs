using System.Security.Claims;

namespace FinanceTracker.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst(ClaimTypes.NameIdentifier)
            ?? principal.FindFirst("sub")
            ?? throw new UnauthorizedAccessException("User ID claim not found.");
        return Guid.Parse(claim.Value);
    }
}
