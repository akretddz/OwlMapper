using Shared.Abstractions.Kernel.Types;

namespace Account.Core.Shared.Entities
{
    internal sealed class AccountToken : AuditableEntity
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
        public string Token { get; set; } = string.Empty;
        public TokenTypeEnum TokenType { get; set; }
        public DateTime Expiration { get; set; }
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
