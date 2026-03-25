namespace Shared.Abstractions.Kernel.Types
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? ModifiedAt { get; set; }
        string? CreatedBy { get; set; }
        string? ModifiedBy { get; set; }
    }
}
