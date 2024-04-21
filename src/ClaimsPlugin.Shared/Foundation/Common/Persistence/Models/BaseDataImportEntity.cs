

using ClaimsPlugin.Shared.Foundation.Common.Utilities;
using ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

public abstract class BaseDataImportEntity<TImportRecordEntity> : BaseAuditableEntity where TImportRecordEntity : BaseDataImportRecordEntity
{
    private readonly List<TImportRecordEntity> _records = new();

    protected BaseDataImportEntity()
    {
        Code = $"{StringUtility.GetUniqueString(6)}";
        TotalRecords = 0;
        NumberOfPendingRecords = 0;
        NumberOfSuccessfulRecords = 0;
        NumberOfFailureRecords = 0;
        NumberOfMissingRecords = 0;
        Status = ImportStatus.Pending;
        ProcessDateTime = DateTime.Now;
    }

    protected BaseDataImportEntity(DateTime? processDateTime, Remarks? remarks)
    {
        Code = $"{StringUtility.GetUniqueString(6)}";
        TotalRecords = 0;
        NumberOfPendingRecords = 0;
        NumberOfSuccessfulRecords = 0;
        NumberOfFailureRecords = 0;
        NumberOfMissingRecords = 0;
        Status = ImportStatus.Pending;
        ProcessDateTime = processDateTime ?? DateTime.Now;
        Remarks = remarks;
    }

    public virtual IReadOnlyCollection<TImportRecordEntity> Records => _records.AsReadOnly();

    public DateTime? ProcessedOn { get; private set; }
    public string Code { get; private set; }
    public int TotalRecords { get; private set; }
    public int NumberOfPendingRecords { get; private set; }
    public int NumberOfSuccessfulRecords { get; private set; }
    public int NumberOfFailureRecords { get; private set; }
    public int NumberOfMissingRecords { get; private set; }
    public ImportStatus Status { get; private set; }
    public Message? Message { get; private set; }

    public Remarks? Remarks { get; private set; }

    public DateTime ProcessDateTime { get; private set; }

    public void UpdateRemarks(Remarks? remarks)
    {
        Remarks = remarks;
    }

    public void UpdateProcessDateTime(DateTime processDateTime)
    {
        if (Status != ImportStatus.Pending)
        {
            throw new DomainException("Unable to update the process date time as the import has been processed.");
        }

        if (processDateTime.IsEarlierThan(DateTime.Now, out string? processDateTimeEarlierThanErrorMessage))
        {
            throw new DomainException(processDateTimeEarlierThanErrorMessage);
        }

        ProcessDateTime = processDateTime;
    }

    public virtual void AddRecord(TImportRecordEntity item)
    {
        TotalRecords = NumberOfPendingRecords += 1;

        _records.Add(item);
    }

    public virtual void Success(int numberOfSuccessfulRecords, int numberOfFailureRecords, Message? message = null)
    {
        NumberOfSuccessfulRecords = numberOfSuccessfulRecords;
        NumberOfFailureRecords = numberOfFailureRecords;
        NumberOfPendingRecords -= numberOfSuccessfulRecords + numberOfFailureRecords;

        NumberOfMissingRecords = TotalRecords - NumberOfSuccessfulRecords - NumberOfFailureRecords - NumberOfPendingRecords;
        Status = ImportStatus.Successful;
        ProcessedOn = DateTime.Now;

        if (message.IsNull())
        {
            Message = Message.Create("The import has been completed successfully.");
        }
    }

    public virtual void Fail(int success, int fail, Message message)
    {
        NumberOfSuccessfulRecords = success;
        NumberOfFailureRecords = fail;
        NumberOfPendingRecords -= success + fail;
        Status = ImportStatus.Failed;
        ProcessedOn = DateTime.Now;
        Message = message;
    }

    public virtual void UpdateStatus(bool success, string message, 
        int successCount = 0, int failCount = 0)
    {
        if (success)
        {
            Success(successCount, failCount, 
                Message.Create(message));
        }
        else
        {
            Fail(successCount, failCount, 
                Message.Create(message));
        }
        
        UpdateRemarks(Remarks.Create(message));
    }
}
