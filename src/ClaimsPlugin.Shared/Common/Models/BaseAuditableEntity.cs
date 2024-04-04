
namespace ClaimsPlugin.Shared.Common.Models
{
    public abstract class BaseAuditableEntity
    {
        public string? InsertBy { get; set; }
        public DateTime? InsertDateTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
