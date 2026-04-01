using Shared.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class AccountIdentity : AuditableEntity
    {
        public Guid AccountId { get; init; }
        public Guid IdentityId { get; init; }
        public Account? Account { get; set; }
        public Identity? Identity { get; set; }
    }
}