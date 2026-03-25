using Shared.Abstractions.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class Role : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
        public string? Name { get; set; }
        public ICollection<AccountRole> AccountRoles { get; init; } = [];
    }
}
