using Shared.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class AccountSecret : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.CreateVersion7();
        public SecretTypeEnum Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
