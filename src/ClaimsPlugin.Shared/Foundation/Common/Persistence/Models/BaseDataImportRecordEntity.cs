using System.ComponentModel.DataAnnotations.Schema;
using ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

public abstract class BaseDataImportRecordEntity
{
    protected BaseDataImportRecordEntity()
    {
        Action = ImportAction.None;

        Status = ImportStatus.Pending;
    }

    protected BaseDataImportRecordEntity(ImportAction action)
    {
        Action = action;

        Status = ImportStatus.Pending;
    }

    [Column(Order = 0)]
    public Guid Id { get; protected set; } = default!;

    public ImportAction Action { get; protected set; } = default!;
    public ImportStatus Status { get; private set; } = default!;
    public Message? Message { get; private set; }
    public Remarks? Remarks { get; private set; }
    public DateTime? ProcessedOn { get; private set; }

    public virtual void Success(Message? message = null, Remarks? remarks = null)
    {
        Status = ImportStatus.Successful;

        Message = message;
        Remarks = remarks;

        ProcessedOn = DateTime.Now;
    }

    public virtual void Fail(Message? message = null, Remarks? remarks = null)
    {
        Status = ImportStatus.Failed;

        Message = message;
        Remarks = remarks;

        ProcessedOn = DateTime.Now;
    }

    public virtual int FailRecord(string message)
    {
        Fail(Message.Create(message), Remarks.Create(message));
        return 1;
    }

    public virtual bool IsFailed()
    {
        return Status == ImportStatus.Failed;
    }
}
