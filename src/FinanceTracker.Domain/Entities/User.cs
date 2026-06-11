using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    public ICollection<Account> Accounts { get; private set; } = new List<Account>();

    private User() { }

    public static User Create(string name, string email, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        return new User
        {
            Name = name,
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash
        };
    }
}
