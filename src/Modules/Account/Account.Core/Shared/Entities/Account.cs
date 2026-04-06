using Shared.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class Account : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.CreateVersion7();
        public string EmailAddress { get; set; } = string.Empty;
        public string? Username { get; set; }
        public AccountIdentity? AccountIdentity { get; set; }
        public ICollection<AccountToken> AccountTokens { get; init; } = [];
        public ICollection<AccountRole> AccountRoles { get; init; } = [];
        public ICollection<AccountSecret>? AccountSecrets { get; set; }
        public bool IsConfirmed { get; set; }
    }
}