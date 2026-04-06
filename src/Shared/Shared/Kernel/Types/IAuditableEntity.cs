namespace Shared.Kernel.Types
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? ModifiedAt { get; set; }
        Guid? CreatedBy { get; set; }
        Guid? ModifiedBy { get; set; }
    }
}