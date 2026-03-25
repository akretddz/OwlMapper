using Shared.Abstractions.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class AccountIdentity : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
        public ICollection<Identity> Identities { get; set; } = [];
    }
}
