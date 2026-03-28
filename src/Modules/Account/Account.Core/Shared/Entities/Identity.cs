using Shared.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class Identity : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.CreateVersion7();
        public IdentityProviderEnum Provider { get; set; }
        public ICollection<AccountIdentity> AccountIdentities { get; set; } = [];
    }
}
