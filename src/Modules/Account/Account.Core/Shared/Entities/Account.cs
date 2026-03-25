using Shared.Abstractions.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class Account : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
        public string? EmailAddress { get; set; }
        public string? Username { get; set; }
        public AccountIdentity? AccountIdentity { get; set; }
        public ICollection<AccountToken> AccountTokens { get; set; } = [];
        public ICollection<AccountRole> AccountRoles { get; set; } = [];
        public AccountSecret? AccountSecret { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
