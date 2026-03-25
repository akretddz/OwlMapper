using Shared.Abstractions.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class AccountRole : AuditableEntity
    {
        public Guid AccountId { get; private init; } = Guid.NewGuid();
        public Guid RoleId { get; set; }
        public Account? Account { get; set; }
        public Role? Role { get; set; }
    }
}
