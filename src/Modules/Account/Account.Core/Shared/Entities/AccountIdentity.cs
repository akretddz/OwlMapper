using Shared.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class AccountIdentity : AuditableEntity
    {
        public Guid IdentityId { get; set; }
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
        public Identity? Identity { get; set; }
    }
}