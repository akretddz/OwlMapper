using Shared.Abstractions.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class AccountSecret : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
