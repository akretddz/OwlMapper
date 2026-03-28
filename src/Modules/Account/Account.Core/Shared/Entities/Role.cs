using Shared.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class Role : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.CreateVersion7();
        public RoleNameEnum? RoleName { get; set; }
        public ICollection<AccountRole> AccountRoles { get; init; } = [];
    }
}
