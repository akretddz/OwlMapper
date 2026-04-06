using Shared.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class AccountRole : AuditableEntity
    {
        public Guid AccountId { get; init; }
        public Guid RoleId { get; init; }
        public Account? Account { get; set; }
        public Role? Role { get; set; }
    }
}