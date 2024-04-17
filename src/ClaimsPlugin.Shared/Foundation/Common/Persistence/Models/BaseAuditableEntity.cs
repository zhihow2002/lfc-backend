using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

public abstract class BaseAuditableEntity : BaseAuditableEntity<Guid>
{
}

public abstract class BaseAuditableEntity<T> : BaseEntity<T>,
    IAuditableEntity,
    ISoftDelete
{
    protected BaseAuditableEntity()
    {
        CreatedOn = DateTime.Now;
        LastModifiedOn = DateTime.Now;
    }

    [Column(Order = 1)]
    public string CreatedBy { get; set; } = default!;

    [Column(Order = 2)]
    public Guid CreatedById { get; set; } = default!;

    [Column(Order = 3)]
    public string CreatedByType { get; set; } = default!;


    [Column(Order = 4)]
    public DateTime CreatedOn { get; private set; }
    
    [Column(Order = 5)]
    public string? LastModifiedBy { get; set; } = default!;

    [Column(Order = 6)]
    public Guid LastModifiedById { get; set; } = default!;

    [Column(Order = 7)]
    public string LastModifiedByType { get; set; } = default!;
    
    [Column(Order = 8)]
    public DateTime? LastModifiedOn { get; set; } = default!;

    [Column(Order = 9)]
    public string? DeletedBy { get; set; } = default!;

    [Column(Order = 10)]
    public Guid? DeletedById { get; set; } = default!;

    [Column(Order = 11)]
    public string? DeletedByType { get; set; } = default!;
    
    [Column(Order = 12)]
    public DateTime? DeletedOn { get; set; } = default!;

}
