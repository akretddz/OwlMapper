using Shared.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class AccountSecret : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.CreateVersion7();
        public Guid AccountId { get; init; }
        public SecretTypeEnum Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public Account? Account { get; set; }
    }
}