using Shared.Abstractions.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class Identity : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
        public IdentityProviderEnum IdentityProvider { get; set; }
        public ICollection<AccountIdentity> AccountIdentities { get; set; } = [];
    }
}
